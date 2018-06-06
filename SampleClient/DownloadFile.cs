using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleClient
{
    class DownloadFile
    {
        public string status { set; get; }
        public string filename { set; get; }
        public double downloadSpeed { set; get; }
        public double uploadSpeed { set; get; }
        public double downloadedRate { set; get; }
        public DownloadFile(string status, string filename, double downloadSpeed, double uploadSpeed, double downloadedRate)
        {
            this.status = status;
            this.filename = filename;
            this.downloadSpeed = downloadSpeed;
            this.uploadSpeed = uploadSpeed;
            this.downloadedRate = downloadedRate;
        }
    }
}
