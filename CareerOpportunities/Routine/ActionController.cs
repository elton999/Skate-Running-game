namespace CareerOpportunities.Routine
{
    public class ActionController
    {
        public enum move {
            NONE,
            FIRE,
            RIGHT,
            LEFT,
            CENTER_UP,
            UP,
            BOTTOM,
            CENTER_BOTTOM
        }
        public float time;
        public move MoveTo;

        public ActionController(move MoveTo, float time)
        {
            this.MoveTo = MoveTo;
            this.time = time;
        }
    }
}
