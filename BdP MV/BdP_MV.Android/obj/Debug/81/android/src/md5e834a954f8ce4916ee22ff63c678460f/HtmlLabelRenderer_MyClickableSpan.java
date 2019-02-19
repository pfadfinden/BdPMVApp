package md5e834a954f8ce4916ee22ff63c678460f;


public class HtmlLabelRenderer_MyClickableSpan
	extends android.text.style.ClickableSpan
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onClick:(Landroid/view/View;)V:GetOnClick_Landroid_view_View_Handler\n" +
			"";
		mono.android.Runtime.register ("LabelHtml.Forms.Plugin.Droid.HtmlLabelRenderer+MyClickableSpan, HtmlLabel.Forms.Plugin", HtmlLabelRenderer_MyClickableSpan.class, __md_methods);
	}


	public HtmlLabelRenderer_MyClickableSpan ()
	{
		super ();
		if (getClass () == HtmlLabelRenderer_MyClickableSpan.class)
			mono.android.TypeManager.Activate ("LabelHtml.Forms.Plugin.Droid.HtmlLabelRenderer+MyClickableSpan, HtmlLabel.Forms.Plugin", "", this, new java.lang.Object[] {  });
	}


	public void onClick (android.view.View p0)
	{
		n_onClick (p0);
	}

	private native void n_onClick (android.view.View p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
