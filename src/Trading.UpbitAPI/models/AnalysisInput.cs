namespace Trading.UpbitAPI
{
    // 최상위 클래스
    public class AnalysisInput
    {
        public PriceData PriceData { get; set; }
        public VolumeData VolumeData { get; set; }
        public Indicators Indicators { get; set; }
        public ChartPatterns ChartPatterns { get; set; }
        public VolumeAnalysis VolumeAnalysis { get; set; }
        public AdditionalData AdditionalData { get; set; }
    }
}
