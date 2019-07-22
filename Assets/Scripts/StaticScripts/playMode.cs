/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

namespace IMRE.HandWaver
{
	/// <summary>
	///     Mode to determine how many tools are loaded at a time.
	///     Meant to integrate with tutorial.
	///     Depreciated.
	/// </summary>
	public static class playMode
    {
	    /// <summary>
	    ///     Boolean controlling if it is currently in demo mode. Use this to pseudo-hide in-progress or buggy features for
	    ///     demos.
	    /// </summary>
	    private static bool _demo;

	    /// <summary>
	    ///     Returns true when in demo mode.
	    /// </summary>
	    public static bool demo
        {
            get => _demo;

            set
            {
                if (modeChangeEvent != null)
                    modeChangeEvent();
                _demo = value;
            }
        }

	    /// <summary>
	    ///     Action called when the mode is changed in and out of demo mode.
	    /// </summary>
	    public static event System.Action modeChangeEvent;
    }
}