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
        public move MoveTo;

        public ActionController(move MoveTo)
        {
            this.MoveTo = MoveTo;
        }
    }
}
