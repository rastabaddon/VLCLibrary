
// This file has been generated by the GUI designer. Do not modify.
namespace VLCLibrary
{
	public partial class VLCWidget
	{
		private global::Gtk.VBox vbox3;
		
		private global::Gtk.Image output;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget VLCLibrary.VLCWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "VLCLibrary.VLCWidget";
			// Container child VLCLibrary.VLCWidget.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Homogeneous = true;
			// Container child vbox3.Gtk.Box+BoxChild
			this.output = new global::Gtk.Image ();
			this.output.Name = "output";
			this.output.Xalign = 0F;
			this.output.Yalign = 0F;
			this.vbox3.Add (this.output);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.output]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			this.Add (this.vbox3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.SizeAllocated += new global::Gtk.SizeAllocatedHandler (this.OnSizeAllocated);
		}
	}
}
