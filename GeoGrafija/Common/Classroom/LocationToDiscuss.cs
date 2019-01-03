namespace Common.Classroom
{
    public class LocationToDiscuss
    {
        public int LocationId { get; set; }
        public string LocationName{ get; set;}

        public DiscussionTopic LocationTopic {get; set;}

        public LocationToDiscuss(){}

        public LocationToDiscuss(bool initWithValues)
        {
            LocationId = 0;
            LocationName = "Нема локација!";
            LocationTopic = new DiscussionTopic(true);
        }
    }
}