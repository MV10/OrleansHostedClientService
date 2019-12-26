# OrleansHostedClientService

In Microsoft Orleans client applications, `IClusterClient` should be closed and disposed at app shutdown. For applications designed around Generic Host, the application lifecycle may not be under the direct control of the code you write. The obvious example is an ASP.NET (or even just Kestrel) web app. Thanks to the convenience of having Generic Host process start-up config and provide dependency injection services, I've started using Generic Host even for quick-and-dirty one-off console tests since I'm usually testing with libraries that are themselves designed around DI (and possibly config).

Under Generic Host, the only way to reliably call `IClusterClient` close/dispose at app shutdown is to register an `IHostedService` and respond to the `StopAsync` method. In this example, SiloApp and TestGrain are trivial (the grain just adds two integers). ClientApp's `Program.Main` does three important things:

* Adds a connected `IClusterClient` as an DI singleton
* Adds a hosted service to cleanly close/dispose the client
* Adds the main program logic as another hosted service

I threw this together after reading Reuben Bond's comment in [this](https://github.com/dotnet/orleans/issues/5758#issuecomment-540158401) Orleans discussion. It is by no means complete -- I believe we'd need support from configuration delegates (similar to `ISiloBuilder`) so that the consumer could identify application parts, handle logging, etc. (see TODO comments in AddClusterClientExtension for some trivial examples).

As for Generic Host, putting the main program logic into a hosted service (`AdditionHostedService`) is only a few extra lines of code, and it cleanly provides DI support. When that logic is done, it signals the Generic Host to terminate the application. When you run the app, the console output shows that the sequence of events looks a bit strange (app shutdown is logged before app start, since the start-up process ends with the shutdown request), but everything works as expected. (Remember this is an Orleans app, you have to run SiloApp before running ClientApp.)

```
Main: AddOrleansClusterClient
Starting AddOrleansClusterClient
Trying to connect, 2 attempts remaining.
Awaiting connect request.
Is client reference null? False
Main: RunConsoleAsync
Main: Add ClusterClientHostedService
Main: Add AdditionHostedService
Cluster client service started.
Addition service started.
Grain reference obtained.
Grain invoked: 10 + 20 = 30
Calling StopApplicaton.
info: Microsoft.Hosting.Lifetime[0]
      Application is shutting down...
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Source\OrleansHostedClientService\ClientApp\bin\Debug\netcoreapp3.1
Cluster client service stopping.
```

