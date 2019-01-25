using System;
using System.Collections.Generic;
using System.Text;

namespace Rcp.Utilities.UsefulObjects
{
    public class PagingResponse<T>
    {
        public PagingResponse()
        { }

        public PagingResponse(string error)
        {
            this.error = error;
        }

        public PagingResponse(List<T> data, int recordsTotal, int draw)
        {
            this.data            = data;
            this.draw            = draw;
            this.recordsFiltered = recordsTotal;
            this.recordsTotal    = recordsTotal;
        }

        public PagingResponse(List<T> data, int recordsTotal, int recordsFiltered, int draw)
        {
            this.data            = data;
            this.draw            = draw;
            this.recordsFiltered = recordsFiltered;
            this.recordsTotal    = recordsTotal;
        }

        public List<T> data { get; set; }

        public int? draw { get; set; }

        public int? recordsTotal { get; set; }

        public int? recordsFiltered { get; set; }

        public string error { get; set; }
    }
}
