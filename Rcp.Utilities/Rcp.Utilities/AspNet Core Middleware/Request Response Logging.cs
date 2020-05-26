using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;

namespace Rcp.Utilities.AspNet_Core_Middleware
{
    /// <summary>
    /// Options for Logging middle ware
    /// </summary>
    public class RequestResponseLoggingMiddlewareOptions
    {
        public RequestResponseLoggingMiddlewareOptions()
        {
            UserNameFunc = context => "None";
            PathsToIgnore = new List<string>();
        }

        public RequestResponseLoggingMiddlewareOptions(Func<HttpContext, string> userNameFunc, IEnumerable<string> pathsToIgnore)
        {
            UserNameFunc  = userNameFunc;
            PathsToIgnore = pathsToIgnore;
        }

        /// <summary>
        /// Used to set the username property of the logging entry
        /// </summary>
        public Func<HttpContext, string> UserNameFunc { get; set; }

        /// <summary>
        /// Paths to ignore logging on.
        /// </summary>
        public IEnumerable<string> PathsToIgnore { get; set; }
    }

    /// <summary>
    /// Logs the request and response using an ILogger injected through the constructor.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _log;
        private readonly Func<HttpContext, string> _userNameFunc;
        private readonly IEnumerable<string> _pathsToIgnore;
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILogger<RequestResponseLoggingMiddleware> logger,
                                                RequestResponseLoggingMiddlewareOptions options)
        {
            _next = next;
            _log = logger;
            _userNameFunc = options.UserNameFunc;
            _pathsToIgnore = options.PathsToIgnore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && _pathsToIgnore.Contains(context.Request.Path.Value, new LambdaComparer<string>(
                                                                                                                                (lhs, rhs) => lhs.Equals(rhs, StringComparison.InvariantCultureIgnoreCase))))
            {
                await _next(context);
                return;
            }


            var request = context.Request;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableRewind();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            request.Body.Read(buffer,
                              0,
                              buffer.Length);

            request.Body.Seek(0,
                              SeekOrigin.Begin);

            //We convert the byte[] into a string using UTF8 encoding...
            var requestBodyText = Encoding.UTF8.GetString(buffer);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                var IsError = false;
                Exception ex = null;
                //...and use that for the temporary response body
                context.Response.Body = responseBody;
                var bob = new List<string>();


                var sw = Stopwatch.StartNew();
                try
                {
                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);
                }
                catch (InvalidOperationException e)
                {
                    if (e.InnerException is ArgumentException && e.InnerException.Source == "System.Private.Xml")
                    {
                        context.Response.StatusCode = 400;
                    }
                    else
                    {
                        IsError = true;
                        ex = e;
                    }
                }
                catch (Exception e)
                {
                    IsError = true;
                    ex = e;
                }

                sw.Stop();

                var response = context.Response;

                //Format the response from the server
                response.Body.Seek(0,
                                   SeekOrigin.Begin);

                //...and copy it into a string
                var responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();

                //We need to reset the reader for the response so that the client can read it.
                response.Body.Seek(0,
                                   SeekOrigin.Begin);

                var initialLevel = LogLevel.Information;
                var logMessage =
                    "HTTP {RequestMethod} {RequestPath} for {UserName} responded {StatusCode} in {Elapsed:0.0000} ms.";

                if (_log.IsEnabled(LogLevel.Debug))
                {
                    initialLevel = LogLevel.Debug;
                    logMessage =
                        "HTTP {RequestMethod} {RequestPath} for {UserName} responded {StatusCode} in {Elapsed:0.0000} ms.  {Request} {Response}";
                }


                //TODO: Save log to chosen datastore
                var statusCode = context.Response?.StatusCode;
                var level = statusCode > 499 ? LogLevel.Error : initialLevel;

                var user = _userNameFunc(context);

                if (IsError)
                {
                    level = LogLevel.Error;

                    logMessage =
                        "HTTP {RequestMethod} {RequestPath} for {UserName} encountered an error returning {StatusCode} in {Elapsed:0.0000} ms.  {Request} {Response}";

                    response.StatusCode = 500;
                }

                var method = context.Request.Method;
                var path = context.Request.Path;

#pragma warning disable 4014
                //Fire and forget the logging.  Probably wont make much difference but why not.
                Task.Run(() => _log.Log(level,
                                        ex,
                                        logMessage,
                                        method,
                                        path,
                                        user ?? "",
                                        statusCode,
                                        sw.Elapsed.TotalMilliseconds,
                                        requestBodyText,
                                        responseBodyText));
#pragma warning restore 4014

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
