using System;
using System.Collections.Generic;
using System.Text;
using Squid;
using GUIControls;

namespace PlanetaryOrbit.Core.Screen
{
    public class ScreenGameDesktop : Desktop
    {
        public DefaultWindow infoWindow;
        public Panel infoPanel;
        public DefaultWindow controlsWindow;
        public Panel controlsPanel;

        public ScreenGameDesktop()
        {
            #region styles

            Skin skin = new Skin();

            ControlStyle baseStyle = new ControlStyle();
            baseStyle.Tiling = TextureMode.Grid;
            baseStyle.Grid = new Margin(3);
            baseStyle.Texture = "System/UI/button_hot.dds";
            baseStyle.Default.Texture = "System/UI/button_default.dds";
            baseStyle.Pressed.Texture = "System/UI/button_down.dds";
            baseStyle.SelectedPressed.Texture = "System/UI/button_down.dds";
            baseStyle.Focused.Texture = "System/UI/button_down.dds";
            baseStyle.SelectedFocused.Texture = "System/UI/button_down.dds";
            baseStyle.Selected.Texture = "System/UI/button_down.dds";
            baseStyle.SelectedHot.Texture = "System/UI/button_down.dds";

            ControlStyle itemStyle = new ControlStyle(baseStyle);
            itemStyle.TextPadding = new Margin(8, 0, 8, 0);
            itemStyle.TextAlign = Alignment.MiddleLeft;

            ControlStyle buttonStyle = new ControlStyle(baseStyle);
            buttonStyle.TextPadding = new Margin(0);
            buttonStyle.TextAlign = Alignment.MiddleCenter;

            ControlStyle tooltipStyle = new ControlStyle(buttonStyle);
            tooltipStyle.TextPadding = new Margin(8);
            tooltipStyle.TextAlign = Alignment.TopLeft;

            ControlStyle inputStyle = new ControlStyle();
            inputStyle.Texture = "System/UI/input_default.dds";
            inputStyle.Hot.Texture = "System/UI/input_focused.dds";
            inputStyle.Focused.Texture = "System/UI/input_focused.dds";
            inputStyle.TextPadding = new Margin(8);
            inputStyle.Tiling = TextureMode.Grid;
            inputStyle.Focused.Tint = ColorInt.RGBA(1, 0, 0, 1);
            inputStyle.Grid = new Margin(3);

            ControlStyle windowStyle = new ControlStyle();
            windowStyle.Tiling = TextureMode.Grid;
            windowStyle.Grid = new Margin(9);
            windowStyle.Texture = "System/UI/window.dds";

            ControlStyle frameStyle = new ControlStyle();
            frameStyle.Tiling = TextureMode.Grid;
            frameStyle.Grid = new Margin(4);
            frameStyle.Texture = "System/UI/frame.dds";
            frameStyle.TextPadding = new Margin(8);

            ControlStyle vscrollTrackStyle = new ControlStyle();
            vscrollTrackStyle.Tiling = TextureMode.Grid;
            vscrollTrackStyle.Grid = new Margin(3);
            vscrollTrackStyle.Texture = "System/UI/vscroll_track.dds";

            ControlStyle vscrollButtonStyle = new ControlStyle();
            vscrollButtonStyle.Tiling = TextureMode.Grid;
            vscrollButtonStyle.Grid = new Margin(3);
            vscrollButtonStyle.Texture = "System/UI/vscroll_button.dds";
            vscrollButtonStyle.Hot.Texture = "System/UI/vscroll_button_hot.dds";
            vscrollButtonStyle.Pressed.Texture = "System/UI/vscroll_button_down.dds";

            ControlStyle vscrollUp = new ControlStyle();
            vscrollUp.Default.Texture = "System/UI/vscrollUp_default.dds";
            vscrollUp.Hot.Texture = "System/UI/vscrollUp_hot.dds";
            vscrollUp.Pressed.Texture = "System/UI/vscrollUp_down.dds";
            vscrollUp.Focused.Texture = "System/UI/vscrollUp_hot.dds";

            ControlStyle hscrollTrackStyle = new ControlStyle();
            hscrollTrackStyle.Tiling = TextureMode.Grid;
            hscrollTrackStyle.Grid = new Margin(3);
            hscrollTrackStyle.Texture = "System/UI/hscroll_track.dds";

            ControlStyle hscrollButtonStyle = new ControlStyle();
            hscrollButtonStyle.Tiling = TextureMode.Grid;
            hscrollButtonStyle.Grid = new Margin(3);
            hscrollButtonStyle.Texture = "System/UI/hscroll_button.dds";
            hscrollButtonStyle.Hot.Texture = "System/UI/hscroll_button_hot.dds";
            hscrollButtonStyle.Pressed.Texture = "System/UI/hscroll_button_down.dds";

            ControlStyle hscrollUp = new ControlStyle();
            hscrollUp.Default.Texture = "System/UI/hscrollUp_default.dds";
            hscrollUp.Hot.Texture = "System/UI/hscrollUp_hot.dds";
            hscrollUp.Pressed.Texture = "System/UI/hscrollUp_down.dds";
            hscrollUp.Focused.Texture = "System/UI/hscrollUp_hot.dds";

            ControlStyle checkButtonStyle = new ControlStyle();
            checkButtonStyle.Default.Texture = "System/UI/checkbox_default.dds";
            checkButtonStyle.Hot.Texture = "System/UI/checkbox_hot.dds";
            checkButtonStyle.Pressed.Texture = "System/UI/checkbox_down.dds";
            checkButtonStyle.Checked.Texture = "System/UI/checkbox_checked.dds";
            checkButtonStyle.CheckedFocused.Texture = "System/UI/checkbox_checked_hot.dds";
            checkButtonStyle.CheckedHot.Texture = "System/UI/checkbox_checked_hot.dds";
            checkButtonStyle.CheckedPressed.Texture = "System/UI/checkbox_down.dds";

            ControlStyle comboLabelStyle = new ControlStyle();
            comboLabelStyle.TextPadding = new Margin(10, 0, 0, 0);
            comboLabelStyle.Default.Texture = "System/UI/combo_default.dds";
            comboLabelStyle.Hot.Texture = "System/UI/combo_hot.dds";
            comboLabelStyle.Pressed.Texture = "System/UI/combo_down.dds";
            comboLabelStyle.Focused.Texture = "System/UI/combo_hot.dds";
            comboLabelStyle.Tiling = TextureMode.Grid;
            comboLabelStyle.Grid = new Margin(3, 0, 0, 0);

            ControlStyle comboButtonStyle = new ControlStyle();
            comboButtonStyle.Default.Texture = "System/UI/combo_button_default.dds";
            comboButtonStyle.Hot.Texture = "System/UI/combo_button_hot.dds";
            comboButtonStyle.Pressed.Texture = "System/UI/combo_button_down.dds";
            comboButtonStyle.Focused.Texture = "System/UI/combo_button_hot.dds";

            ControlStyle multilineStyle = new ControlStyle();
            multilineStyle.TextAlign = Alignment.TopLeft;
            multilineStyle.TextPadding = new Margin(8);

            ControlStyle labelStyle = new ControlStyle();
            labelStyle.TextPadding = new Margin(8, 0, 8, 0);
            labelStyle.TextAlign = Alignment.MiddleLeft;
            labelStyle.TextColor = ColorInt.RGBA(.8f, .8f, .8f, 1);
            labelStyle.BackColor = ColorInt.RGBA(1, 1, 1, .125f);
            labelStyle.Default.BackColor = 0;

            skin.Styles.Add("item", itemStyle);
            skin.Styles.Add("textbox", inputStyle);
            skin.Styles.Add("button", buttonStyle);
            skin.Styles.Add("window", windowStyle);
            skin.Styles.Add("frame", frameStyle);
            skin.Styles.Add("checkBox", checkButtonStyle);
            skin.Styles.Add("comboLabel", comboLabelStyle);
            skin.Styles.Add("comboButton", comboButtonStyle);
            skin.Styles.Add("vscrollTrack", vscrollTrackStyle);
            skin.Styles.Add("vscrollButton", vscrollButtonStyle);
            skin.Styles.Add("vscrollUp", vscrollUp);
            skin.Styles.Add("hscrollTrack", hscrollTrackStyle);
            skin.Styles.Add("hscrollButton", hscrollButtonStyle);
            skin.Styles.Add("hscrollUp", hscrollUp);
            skin.Styles.Add("multiline", multilineStyle);
            skin.Styles.Add("tooltip", tooltipStyle);
            skin.Styles.Add("label", labelStyle);

            GuiHost.SetSkin(skin);

            #endregion

            #region cursors

            Point cursorSize = new Point(32, 32);
            Point halfSize = cursorSize / 2;

            skin.Cursors.Add(Cursors.Default, new Cursor { Texture = "System/UI/Cursors/Arrow.png", Size = cursorSize, HotSpot = Point.Zero });
            skin.Cursors.Add(Cursors.Link, new Cursor { Texture = "System/UI/Cursors/Link.png", Size = cursorSize, HotSpot = Point.Zero });
            skin.Cursors.Add(Cursors.Move, new Cursor { Texture = "System/UI/Cursors/Move.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.Select, new Cursor { Texture = "System/UI/Cursors/Select.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.SizeNS, new Cursor { Texture = "System/UI/Cursors/SizeNS.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.SizeWE, new Cursor { Texture = "System/UI/Cursors/SizeWE.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.HSplit, new Cursor { Texture = "System/UI/Cursors/SizeNS.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.VSplit, new Cursor { Texture = "System/UI/Cursors/SizeWE.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.SizeNESW, new Cursor { Texture = "System/UI/Cursors/SizeNESW.png", Size = cursorSize, HotSpot = halfSize });
            skin.Cursors.Add(Cursors.SizeNWSE, new Cursor { Texture = "System/UI/Cursors/SizeNWSE.png", Size = cursorSize, HotSpot = halfSize });

            #endregion

            #region main

            TooltipControl = new SimpleTooltip();

            #region sample window 5 - Panel, TextBox

            controlsWindow = new DefaultWindow();
            controlsWindow.Size = new Point(500, 500);
            controlsWindow.Position = new Point(0, 0);
            controlsWindow.Resizable = false;
            controlsWindow.Parent = this;
            controlsWindow.Opacity = 0.6f;
            controlsWindow.Titlebar.Text = "How To: Controls";

            controlsPanel = new Panel();
            controlsPanel.Style = "frame";
            controlsPanel.Dock = DockStyle.Fill;
            controlsPanel.Parent = controlsWindow;
            controlsPanel.ClipFrame.Margin = new Margin(2);
            controlsPanel.ClipFrame.Style = "textbox";

            Label controlsDescription = new Label();
            controlsDescription.Text = "-----------------\nCamera (General)\n-----------------\nReset Camera = Spacebar\nChase Camera = F2\nOrbit Camera = F3\nTop Camera = F4\nCamera Zoom (In/Out) = +/- or x/z\n-----------------\nCamera (Chase)\n-----------------\nPlanet Select Next/Previous = Right Arrow Key/Left Arrow Key\n-----------------\nCamera (Orbit)\n-----------------\nMove Camera = W/A/S/D\n-----------------\nSimulation\n-----------------\nToggle Visual Orbits/More Information = F1\nPause Simulation = F6\nSimulation Speed Decrease/Increase = F7/F8\n";
            controlsDescription.Position = new Point(0, 0);
            controlsDescription.Size = new Squid.Point(485, 455);
            controlsDescription.Style = "textbox";
            controlsDescription.AllowFocus = false;
            controlsDescription.TextWrap = true;
            controlsPanel.Content.Controls.Add(controlsDescription);

            infoWindow = new DefaultWindow();
            infoWindow.Size = new Point(300, 340);
            infoWindow.Resizable = false;
            infoWindow.Dock = DockStyle.Right;
            infoWindow.Titlebar.Text = "Object Information";
            infoWindow.Parent = this;
            infoWindow.Opacity = 0.6f;

            infoPanel = new Panel();
            infoPanel.Style = "frame";
            infoPanel.Dock = DockStyle.Fill;
            infoPanel.Parent = infoWindow;

            infoPanel.ClipFrame.Margin = new Margin(2);
            infoPanel.ClipFrame.Style = "textbox";

            infoPanel.VScroll.Margin = new Margin(0, 8, 8, 8);
            infoPanel.VScroll.Size = new Squid.Point(14, 10);
            infoPanel.VScroll.Slider.Style = "vscrollTrack";
            infoPanel.VScroll.Slider.Button.Style = "vscrollButton";
            infoPanel.VScroll.ButtonUp.Style = "vscrollUp";
            infoPanel.VScroll.ButtonUp.Size = new Squid.Point(10, 20);
            infoPanel.VScroll.ButtonDown.Style = "vscrollUp";
            infoPanel.VScroll.ButtonDown.Size = new Squid.Point(10, 20);
            infoPanel.VScroll.Slider.Margin = new Margin(0, 2, 0, 2);

            Label planetName = new Label();
            planetName.Text = "Name:";
            planetName.Size = new Point(130, 35);
            planetName.Position = new Point(10, 10 + 45 * 0);
            infoPanel.Content.Controls.Add(planetName);

            TextBox planetNameTXT = new TextBox();
            planetNameTXT.Text = "";
            planetNameTXT.Size = new Squid.Point(130, 35);
            planetNameTXT.Position = new Point(140, 10 + 45 * 0);
            planetNameTXT.Style = "textbox";
            planetNameTXT.AllowFocus = false;
            planetNameTXT.ReadOnly = true;
            infoPanel.Content.Controls.Add(planetNameTXT);

            Label planetVelocity = new Label();
            planetVelocity.Text = "Velocity:";
            planetVelocity.Size = new Point(130, 35);
            planetVelocity.Position = new Point(10, 10 + 45 * 1);
            infoPanel.Content.Controls.Add(planetVelocity);

            TextBox planetVelocityTXT = new TextBox();
            planetVelocityTXT.Text = "";
            planetVelocityTXT.Size = new Squid.Point(130, 35);
            planetVelocityTXT.Position = new Point(140, 10 + 45 * 1);
            planetVelocityTXT.Style = "textbox";
            planetVelocityTXT.AllowFocus = false;
            planetVelocityTXT.ReadOnly = true;
            infoPanel.Content.Controls.Add(planetVelocityTXT);


            Label planetDistance = new Label();
            planetDistance.Text = "Distance:";
            planetDistance.Size = new Point(130, 35);
            planetDistance.Position = new Point(10, 10 + 45 * 2);
            infoPanel.Content.Controls.Add(planetDistance);

            TextBox planetDistanceTXT = new TextBox();
            planetDistanceTXT.Text = "";
            planetDistanceTXT.Size = new Squid.Point(130, 35);
            planetDistanceTXT.Position = new Point(140, 10 + 45 * 2);
            planetDistanceTXT.Style = "textbox";
            planetDistanceTXT.AllowFocus = false;
            planetDistanceTXT.ReadOnly = true;
            infoPanel.Content.Controls.Add(planetDistanceTXT);

            Label planetPeriod = new Label();
            planetPeriod.Text = "Orbit Period:";
            planetPeriod.Size = new Point(130, 35);
            planetPeriod.Position = new Point(10, 10 + 45 * 3);
            infoPanel.Content.Controls.Add(planetPeriod);

            TextBox planetPeriodTXT = new TextBox();
            planetPeriodTXT.Text = "";
            planetPeriodTXT.Size = new Squid.Point(130, 35);
            planetPeriodTXT.Position = new Point(140, 10 + 45 * 3);
            planetPeriodTXT.Style = "textbox";
            planetPeriodTXT.AllowFocus = false;
            planetPeriodTXT.ReadOnly = true;
            infoPanel.Content.Controls.Add(planetPeriodTXT);

            Label planetDescription = new Label();
            planetDescription.Text = "Information:";
            planetDescription.Size = new Point(260, 35);
            planetDescription.Position = new Point(10, 10 + 45 * 4);
            infoPanel.Content.Controls.Add(planetDescription);

            Label planetDescriptionTXT = new Label();
            planetDescriptionTXT.Text = "";
            planetDescriptionTXT.Size = new Squid.Point(260, 35);
            planetDescriptionTXT.Position = new Point(10, 10 + 45 * 5);
            planetDescriptionTXT.Style = "textbox";
            planetDescriptionTXT.AllowFocus = false;
            planetDescriptionTXT.TextWrap = true;
            planetDescriptionTXT.AutoSize = Squid.AutoSize.Vertical;
            infoPanel.Content.Controls.Add(planetDescriptionTXT);

            #endregion

            #endregion
        }

        public class MyData
        {
            public string Name;
            public DateTime Date;
            public int Rating;
        }

        void view_ColumnClicked(object sender, GridViewEventArgs e)
        {
            GridView view = sender as GridView;
            GridColumn col = e.Column;

            if (col.Tag == null) col.Tag = 0;

            int tag = (int)col.Tag;
            col.Tag = tag = 1 - tag;

            if (col.Index == 0)
            {
                if (tag == 0)
                    view.Items.Sort((a, b) => a.Text.CompareTo(b.Text));
                else
                    view.Items.Sort((b, a) => a.Text.CompareTo(b.Text));
            }
            else
            {
                if (tag == 0)
                    view.Items.Sort((a, b) => a.SubItems[col.Index - 1].Text.CompareTo(b.SubItems[col.Index - 1].Text));
                else
                    view.Items.Sort((b, a) => a.SubItems[col.Index - 1].Text.CompareTo(b.SubItems[col.Index - 1].Text));
            }
        }

        void txt_OnGotFocus(Control sender)
        {
            TextBox txt = sender as TextBox;
            txt.Select(0, txt.Text.Length);
        }

        void item_OnMouseLeave(Control sender)
        {
            sender.Animation.Stop();
            sender.Animation.Size(new Point(100, 70), 250);
        }

        void item_OnMouseEnter(Control sender)
        {
            sender.Animation.Stop();
            sender.Animation.Size(new Point(100, 140), 250);
        }

        void b1_OnMouseClick(Control sender, MouseEventArgs args)
        {
            ControlStyle style = GuiHost.GetStyle("multiline");
            style.TextAlign = (Alignment)sender.Tag;
        }

        void lbl_OnLinkClicked(string href)
        {
            MessageBox dialog = MessageBox.Show(new Point(300, 200), "Message Box", href, MessageBoxButtons.OKCancel, this);
            dialog.OnResult += new Dialog.DialogResultEventHandler(dialog_OnDialogResult);
            dialog.Animation.Custom(WalkSquare(dialog));
        }

        private System.Collections.IEnumerator WalkSquare(MessageBox dialog)
        {
            yield return dialog.Animation.Position(new Point(10, 10), 1000);
            yield return dialog.Animation.Position(new Point(1000, 10), 1000);
            yield return dialog.Animation.Position(new Point(1000, 600), 1000);
            yield return dialog.Animation.Position(new Point(10, 600), 1000);
        }

        void label1_OnMouseDown(Control sender, MouseEventArgs args)
        {
            Button btn = new Button();
            btn.Size = new Squid.Point(157, 26);
            btn.Text = "drag me";
            btn.Position = sender.Location;
            sender.DoDragDrop(btn);
        }

        void txt_OnDragDrop(Control sender, DragDropEventArgs e)
        {
            if (e.Source is Label)
            {
                ((TextBox)sender).Text = ((Button)e.DraggedControl).Text;
            }
        }

        void button_OnMouseClick(Control sender, MouseEventArgs args)
        {
            MessageBox dialog = MessageBox.Show(new Point(300, 200), "Message Box", "This is a modal Dialog.", MessageBoxButtons.OKCancel, this);
            dialog.OnResult += dialog_OnDialogResult;
        }

        void dialog_OnDialogResult(Dialog sender, DialogResult result)
        {
            // do something
        }
    }
}
