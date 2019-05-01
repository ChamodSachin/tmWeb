using System.Web.Http;
using TrueMarbleLibrary;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Net;

namespace TMWeb.Contollers
{

    public class TrueMarbleController : ApiController{

        /* Fuction: Post
         * Type: POST
         * Parameters: zoom level, x coordinate, y coordinate
         * Return: byte[]
         * Assertion:  function returns loaded tile images
         */
        [HttpPost]
        [Route("View")]
        public byte[] Post()
        {
            try
            {
                Dictionary<string, object> req = (Dictionary<string, object>)Request.Content.ReadAsAsync<Dictionary<string, object>>().Result;
                int zoom = Convert.ToInt32(req["zoom"]);
                int x = Convert.ToInt32(req["x"]);
                int y = Convert.ToInt32(req["y"]);
                int bufSize = x * y * 3;
                byte[] imageBuf = new byte[bufSize];
                TrueMarble.GetTileImageAsRawJPG(zoom, x, y, out imageBuf, bufSize, out int jpgSize);
                return imageBuf;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(HttpStatusCode.InternalServerError, "Error occurred : " + e.Message));
            }
        }


        /* Fuction: NumOfTiles
         * Type: POST
         * Parameters:  
         * Return: byte[]
         * Assertion:  function returns the number of tiles across and the number of tiles down
         */
        [HttpGet]
        [Route("Tiles/NumOfTiles/{zoom1}")]
        public Dictionary<string, int> NumOfTiles(int zoom1)
        {
            try
            {
                Dictionary<string, int> req1 = new Dictionary<string, int>();
                int numTilesX, numTilesY;
                if ((TrueMarble.GetNumTiles(zoom1, out numTilesX, out numTilesY) == 1))
                {
                    req1.Add("across", numTilesX);
                    req1.Add("down", numTilesY);
                    return req1;
                }
                else
                {
                    req1.Add("across", 1);
                    req1.Add("down", 0);
                    return req1;
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(this.Request.CreateResponse<object>(HttpStatusCode.InternalServerError, "Error : " + e.Message));
            }
        }

        [HttpGet]
        [Route("Tiles/levels")]
        public int Levels()
        {
            return 6;        
        }
    }


}