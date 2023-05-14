using Uniza.Namedays;
using Uniza.Namedays.ViewerConsoleApp;

FileInfo fileInfo = new FileInfo("namedays-sk.csv");

ConsoleViewer viewer = new ConsoleViewer(fileInfo);
viewer.MainLoop();
