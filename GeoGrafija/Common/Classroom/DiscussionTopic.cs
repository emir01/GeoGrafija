namespace Common.Classroom
{
    public class DiscussionTopic
    {
        public string Title { get; set;}
        public string Text { get; set;}

        public DiscussionTopic(){}
       
        public DiscussionTopic(bool initWithValues)
        {
            Title = "Пример Наслов";
            Text = "Пример Текст";
        }
    }
}