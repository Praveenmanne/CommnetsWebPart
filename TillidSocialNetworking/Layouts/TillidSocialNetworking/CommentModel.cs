using System;

namespace TillidSocialNetworking
{
    [Serializable]
    public class CommentModel
    {
        public string ImageUrl { get; set; }

        public string PostedBy { get; set; }

        public DateTime PostedOn { get; set; }

        public string Comment { get; set; }

        public string PostedDifference { get; set; }
        public string ProfileURl { get; set; }
        
        public string Postedon { get; set; }

        public string ID { get; set; }
        
    }
}