﻿
namespace GMap.NET.MapProviders
{
   using System;

    /// <summary>
    /// GoogleCnHybridMap provider
    /// </summary>
    public class GoogleCnHybridMapProvider : GoogleMapProviderBase
    {
        public static readonly GoogleCnHybridMapProvider Instance;

        GoogleCnHybridMapProvider()
        {
            RefererUrl = string.Format("http://www.google.cn/");
        }

        static GoogleCnHybridMapProvider()
        {
            Instance = new GoogleCnHybridMapProvider();
        }

        public string Version = "h@298";

        #region GMapProvider Members

        readonly Guid id = new Guid("16B52F7D-5124-4C8C-8552-0CAE747E617D");
        public override Guid Id
        {
            get
            {
                return id;
            }
        }

        readonly string name = "GoogleCnHybridMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { GoogleCnSatelliteMapProvider.Instance, this };
                }
                return overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        #endregion

        internal void GetSecureWords(GPoint pos, out string sec1, out string sec2)
        {
            sec1 = string.Empty; // after &x=...
            sec2 = string.Empty; // after &zoom=...
            int seclen = (int)((pos.X * 3) + pos.Y) % 8;
            sec2 = SecureWord.Substring(0, seclen);
            if (pos.Y >= 10000 && pos.Y < 100000)
            {
                sec1 = Sec1;
            }
        }
        static readonly string Sec1 = "&s=";

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            string sec1 = string.Empty; // after &x=...
            string sec2 = string.Empty; // after &zoom=...
            GetSecureWords(pos, out sec1, out sec2);

            return string.Format(UrlFormat, UrlFormatServer, GetServerNum(pos, 1), UrlFormatRequest, Version, ChinaLanguage, pos.X, sec1, pos.Y, zoom, sec2, ServerChina);
        }

        static readonly string ChinaLanguage = "zh-CN";
        static readonly string UrlFormatServer = "mt";
        static readonly string UrlFormatRequest = "vt";
        static readonly string UrlFormat = "http://{0}{1}.{10}/{2}/imgtp=png32&lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}";
    }
}