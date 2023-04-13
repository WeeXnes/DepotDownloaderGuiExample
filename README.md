<div align="center">
    <img width="100" height="100" src="https://cdn.discordapp.com/attachments/741123537582162020/965619554426437732/wicon.png">
<h1 align="center">DepotDownloaderGuiExample</h1>
</div>

<h3>MainWindow.cs</h3>

```cs
public MainWindow()
{
    InitializeComponent();
    //Intercepting DepotDownloaders Output and Input
    //Display current download progress in ProgressBar (Invoke needed cause its being accessed from a background worker)
    DepotDownloaderLib.onConsoleOutput += (f, s) =>
    {
        Dispatcher.Invoke(() =>
        {
            downloadProgress.Value = f;
        });
    };
    //Handling the request for the 2FA code
    DepotDownloaderLib.onConsoleInput += () =>
    {
        string inputRead = (string)Dispatcher.Invoke(() =>
        {
            //Creating a InputBox window to get user input for the 2FA Code
            return new InputBox("Enter 2FA Code").ShowDialog();
        });
        return inputRead;
    };
}

private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
{
    List<DownloaderArgument> arguments = new List<DownloaderArgument>();
    arguments.Add(new DownloaderArgument(ArgType.AppId, appId.Text));
    arguments.Add(new DownloaderArgument(ArgType.DepotId, depotId.Text));
    arguments.Add(new DownloaderArgument(ArgType.ManifestId, manifestId.Text));
    arguments.Add(new DownloaderArgument(ArgType.Username, loginName.Text));
    arguments.Add(new DownloaderArgument(ArgType.Password, loginPassword.Text));
    arguments.Add(new DownloaderArgument(ArgType.Directory, directory.Text));
    arguments.Add(new DownloaderArgument(ArgType.MaxDownloads, "10"));
    DepotDownloaderLib.StartDownload(arguments, true);
}
```

<h3>InputBox.cs (Example 2FA Input Window)</h3>
This is not 100% my code, this is an altered version of code i found on StackOverflow

```cs

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public class InputBox
{
    Window Box = new Window();//window for the inputbox
    FontFamily font = new FontFamily("Avenir");//font for the whole inputbox
    int FontSize = 14;//fontsize for the input
    StackPanel sp1 = new StackPanel();// items container
    string title = "Enter Two Factor Code";//title as heading
    string boxcontent;//title
    string defaulttext = "";//default textbox content
    string errormessage = "";//error messagebox content
    string errortitle = "Error";//error messagebox heading title
    string okbuttontext = "OK";//Ok button content
    string CancelButtonText = "Cancel";
    Brush BoxBackgroundColor = Brushes.WhiteSmoke;// Window Background
    Brush InputBackgroundColor = Brushes.Ivory;// Textbox Background
    bool clickedOk = false;
    TextBox input = new TextBox();
    Button ok = new Button();
    Button cancel = new Button();
    bool inputreset = false;


    public InputBox(string content)
    {
        try
        {
            boxcontent = content;
        }
        catch { boxcontent = "Error!"; }
        windowdef();
    }

    public InputBox(string content, string Htitle, string DefaultText)
    {
        try
        {
            boxcontent = content;
        }
        catch { boxcontent = "Error!"; }
        try
        {
            title = Htitle;
        }
        catch
        {
            title = "Error!";
        }
        try
        {
            defaulttext = DefaultText;
        }
        catch
        {
            DefaultText = "Error!";
        }
        windowdef();
    }

    public InputBox(string content, string Htitle, string Font, int Fontsize)
    {
        try
        {
            boxcontent = content;
        }
        catch { boxcontent = "Error!"; }
        try
        {
            font = new FontFamily(Font);
        }
        catch { font = new FontFamily("Tahoma"); }
        try
        {
            title = Htitle;
        }
        catch
        {
            title = "Error!";
        }
        if (Fontsize >= 1)
            FontSize = Fontsize;
        windowdef();
    }

    private void windowdef()// window building - check only for window size
    {
        Box.Height = 100;// Box Height
        Box.Width = 450;// Box Width
        Box.Background = BoxBackgroundColor;
        Box.Title = title;
        Box.Content = sp1;
        Box.Closing += Box_Closing;
        Box.WindowStyle = WindowStyle.None;
        Box.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        TextBlock content = new TextBlock();
        content.TextWrapping = TextWrapping.Wrap;
        content.Background = null;
        content.HorizontalAlignment = HorizontalAlignment.Center;
        content.Text = boxcontent;
        content.FontFamily = font;
        content.FontSize = FontSize;
        sp1.Children.Add(content);

        input.Background = InputBackgroundColor;
        input.FontFamily = font;
        input.FontSize = FontSize;
        input.HorizontalAlignment = HorizontalAlignment.Center;
        input.Text = defaulttext;
        input.MinWidth = 200;
        input.MouseEnter += input_MouseDown;
        input.KeyDown += input_KeyDown;

        sp1.Children.Add(input);

        ok.Width = 70;
        ok.Height = 30;
        ok.Click += ok_Click;
        ok.Content = okbuttontext;

        cancel.Width = 70;
        cancel.Height = 30;
        cancel.Click += cancel_Click;
        cancel.Content = CancelButtonText;

        WrapPanel gboxContent = new WrapPanel();
        gboxContent.HorizontalAlignment = HorizontalAlignment.Center;

        sp1.Children.Add(gboxContent);
        gboxContent.Children.Add(ok);
        gboxContent.Children.Add(cancel);

        input.Focus();
    }

    void Box_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
       //validation
    }

    private void input_MouseDown(object sender, MouseEventArgs e)
    {
        if ((sender as TextBox).Text == defaulttext && inputreset == false)
        {
            (sender as TextBox).Text = null;
            inputreset = true;
        }
    }

    private void input_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && clickedOk == false )
        {
            e.Handled = true;
            ok_Click(input, null);
        }

        if (e.Key == Key.Escape)
        {
            cancel_Click(input, null);
        }
    }

    void ok_Click(object sender, RoutedEventArgs e)
    {
        clickedOk = true;
        if (input.Text == defaulttext || input.Text == "")
            MessageBox.Show(errormessage, errortitle,MessageBoxButton.OK,MessageBoxImage.Error);
        else
        {
            Box.Close();
        }
        clickedOk = false;
    }

    void cancel_Click(object sender, RoutedEventArgs e)
    {
        Box.Close();
    }

    public string ShowDialog()
    {
        Box.ShowDialog();
        return input.Text;
    }
}
```
