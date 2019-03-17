namespace TrainApp.Core.Entity
{
    public class StepModel
    {
        public int ID { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string Travel_mode { get; set; }
        public TransitDetailModel Transit { get; set; }
        public int Seats { get; set; }
        public double Price { get; set; }
    }
}
