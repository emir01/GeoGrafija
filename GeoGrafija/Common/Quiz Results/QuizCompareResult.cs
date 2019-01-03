namespace Common.Quiz_Results
{
    public  class QuizCompareResult
    {
        public int CorrectQuestions { get; set; }
        public int StudentPoints { get; set; }

        public QuizDetailedResults QuizDetailResults { get; set; }

        public QuizCompareResult()
        {
            QuizDetailResults = new QuizDetailedResults();
        }
    }
}