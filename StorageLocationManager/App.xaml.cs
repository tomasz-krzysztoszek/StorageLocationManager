using Application.Services;
using Autofac;
using Autofac.Extras.NLog;
using Autofac.Features.ResolveAnything;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using DAO.DataModels;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using LinqToDB.Data;
using Model;
using Model.Helpers;
using Model.Settings;
using Newtonsoft.Json;
using Repository;
using Repository.DAO;
using Repository.Helpers;
using StorageLocationManager.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ViewModel;
using ViewModel.Converters;
using ViewModel.Extensions;
using ViewModel.Helpers;

namespace StorageLocationManager
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public class AppSettingsRoot
        {
            public AppSettings AppSettings { get; set; }
            public PcbRestParam PcbRestSerwerParamData { get; set; }
        }
        private IContainer container;
        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreenManager.CreateThemed(new DXSplashScreenViewModel
            {
                Logo = null,
                Copyright = "TSK Soft. All rights reserved",
                IsIndeterminate = true,
                Status = "Trwa Uruchamianie...",
                Title = "STORAGE LOCATION MANAGER",
                Subtitle = "Powered by DEV EXPRESS"
            }
            ).ShowOnStartup();

            var userData = new User();
            var appSettingsRoot = new AppSettingsRoot();
            var appProfileInstance = new AppProfile();

            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string appSettings = r.ReadToEnd();
                appSettingsRoot = JsonConvert.DeserializeObject<AppSettingsRoot>(appSettings);
            }

            using (StreamReader r = new StreamReader("appprofile.json"))
            {
                string appProfile = r.ReadToEnd();
                appProfileInstance = JsonConvert.DeserializeObject<AppProfile>(appProfile);
            }

            var appSettingsInstance = appSettingsRoot.AppSettings;
            var pcbRestParamInstance = appSettingsRoot.PcbRestSerwerParamData;

            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterType<FBDBDB>()
                .UsingConstructor(typeof(string))
                .WithParameter("configuration", "FirebirdConnectionString")
                .SingleInstance();
            builder.RegisterType<LoginWindow>().SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<CustomNotifier>().SingleInstance();
            builder.RegisterType<StatusToColorConverter>().SingleInstance();
            builder.RegisterType<SessionContext>().As<ISessionContext>().SingleInstance();
            builder.RegisterType<AritMscSklRepository>().As<IAritMscSklRepository>();
            builder.RegisterType<AritRackRepository>().As<IAritRackRepository>();
            builder.RegisterType<AritDostawaRepository>().As<IAritDostawaRepository>();
            builder.RegisterType<AritUzytkownikRepository>().As<IAritUzytkownikRepository>();
            builder.RegisterType<MscSklService>().As<IMscSklService>();
            builder.RegisterType<RackService>().As<IRackService>();
            builder.RegisterType<EditMscSklService>().As<IEditMscSklService>();
            builder.RegisterType<EditSettingsService>().As<IEditSettingsService>();
            builder.RegisterType<EditMscSklViewModel>().As<IEditMscSklViewModel>();
            builder.RegisterType<EditSettingsViewModel>().As<IEditSettingsViewModel>();
            builder.RegisterType<PrintDocService>().As<IPrintDocService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PdfViewerViewModel>().As<IPdfViewerViewModel>();
            builder.RegisterModule<NLogModule>();
            builder.RegisterAutoMapper(typeof(AutoMappingProfile).Assembly);
            builder.Register(c => appSettingsInstance).SingleInstance();
            builder.Register(c => pcbRestParamInstance).SingleInstance();
            builder.Register(c => appProfileInstance).SingleInstance();
            builder.Register(c => userData).SingleInstance();
            container = builder.Build();

            DISource.Resolver = (type) =>
            {
                return container.Resolve(type);
            };

            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (s1, s2, s3) =>
            {
                Debug.WriteLine(s1);
                Debug.WriteLine(s2);
            };

            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var statusToColorConverter = container.Resolve<StatusToColorConverter>();
            var loginWindow = container. Resolve<LoginWindow>();
            loginWindow.Show();
            loginWindow.IsVisibleChanged += (obj, evArgs) =>
            {
                if(loginWindow.IsVisible == false && loginWindow.IsLoaded)
                {
                    var mainWindow = container.Resolve<MainWindow>();
                    var notifier = container.Resolve<CustomNotifier>();
                    notifier.SetNewInstance(CreateNotifier(mainWindow));
                    mainWindow.Show();
                    loginWindow.Close();
                }
            };
        }

        private Notifier CreateNotifier(Window baseWindow)
        {
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: baseWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Dispatcher.CurrentDispatcher;
            });

            return notifier;
        }
    }
}
