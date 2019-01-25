using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Rcp.Utilities
{
    public static class GenericBatching
    {
        /// <summary>
        /// This will go till the token is canceled
        /// </summary>
        /// <typeparam name="TBatchItem"></typeparam>
        /// <param name="token"></param>
        /// <param name="workQueue"></param>
        /// <param name="log"></param>
        /// <param name="listOperation"></param>
        /// <param name="maxRetryReachedExceptionHandler"></param>
        /// <param name="fatalExceptionHandler"></param>
        /// <param name="maxSaveDelay"></param>
        /// <param name="maxRecords"></param>
        /// <param name="maxRetries"></param>
        /// <param name="retryWaitSeconds"></param>
        /// <param name="tryTakeWaitSeconds"></param>
        public static void BatchingConsumer<TBatchItem>(
            CancellationToken token,
            BlockingCollection<TBatchItem> workQueue,
            Action<string, Exception> log,
            Action<IEnumerable<TBatchItem>> listOperation,
            Action<Exception, IEnumerable<TBatchItem>> maxRetryReachedExceptionHandler,
            Action<Exception, IEnumerable<TBatchItem>> fatalExceptionHandler,
            int maxSaveDelay,
            int maxRecords = 1000,
            int maxRetries = 600,
            int retryWaitSeconds = 1,
            int tryTakeWaitSeconds = 1)
        {
            var operatingListSize = maxRecords + 5;

            var list = new List<TBatchItem>(operatingListSize);

            try
            {
                var lastSave = DateTime.Now;
                var counter = 0;

                while (!token.IsCancellationRequested)
                {

                    try
                    {
                        TBatchItem outVar;

                        while (!token.IsCancellationRequested &&
                               //check if the counter is greater than max records then increment
                               counter++ < maxRecords &&
                               //Make sure we save every X seconds no matter what
                               lastSave.AddSeconds(maxSaveDelay) > DateTime.Now &&
                               //Make sure we take something
                               //TryTake blocks till the wait time passes or the token is canceled
                               workQueue.TryTake(out outVar, tryTakeWaitSeconds * 1000, token))
                        {
                            list.Add(outVar);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //The Token is canceled while waiting.  Dont care since we expect this to happen every now and then when stopping.
                    }

                    if (list.Count > 0)
                    {
                        var retry = true;
                        var retryCount = 0;

                        do
                        {
                            try
                            {
                                listOperation(list);

                                retry = false;
                            }
                            catch (Exception ex)
                            {
                                retryCount++;

                                if (retryCount >= maxRetries)
                                {
                                    maxRetryReachedExceptionHandler(ex,
                                                                    list);

                                    retry = false;
                                }
                                else if (token.IsCancellationRequested)
                                {
                                    throw;
                                }
                                else
                                {
                                    log("Error occurred in batching, retrying operation after wait delay.", ex);

                                    token.WaitHandle.WaitOne(retryWaitSeconds * 1000);
                                }
                            }
                        } while (retry);
                    }

                    //reset our counters.
                    list = new List<TBatchItem>(operatingListSize);
                    counter = 0;
                    lastSave = DateTime.Now;
                }

                //Token is canceled
                //clean up the loop

                //Make sure we get all the items
                foreach (var item in workQueue.GetConsumingEnumerable())
                {
                    list.Add(item);
                }

                //save all the changes.
                listOperation(list);
            }
            catch (Exception ex)
            {
                fatalExceptionHandler(ex,
                                      list);
            }
        }
    }
}
