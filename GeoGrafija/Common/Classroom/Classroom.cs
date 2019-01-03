using System;
using System.Collections.Generic;

namespace Common.Classroom
{
    public class Classroom
    {
        public string Title {get;set;}
        public DateTime LastEdit { get; set; }
        public LocationToDiscuss MainLocation { get; set; }
        public List<LocationToDiscuss> SecondaryLocations { get; set; }

        public DiscussionTopic MainDiscussionTopic { get; set; }
        public List<DiscussionTopic> SecondaryDiscussionTopics { get; set; }

        public Classroom()
        {

        }

        public Classroom(bool initValues)
        {
            Title = "Училница";
            LastEdit = DateTime.Now;
            MainLocation = new LocationToDiscuss(true);
            MainDiscussionTopic = new DiscussionTopic(true);

            SecondaryLocations = new List<LocationToDiscuss>();
            SecondaryDiscussionTopics = new List<DiscussionTopic>();
        }
    }
}