const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jobshub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();

connection.on("MonitorAlertSchedulerConfig", function (message) {
    var li = document.createElement("li");
    li.className = "notification-item";
    li.append('<i class="bi bi-check-circle text-success"></i><div><h4>Job Notification</h4><p>Quae dolorem earum veritatis oditseno</p> <p>30 min. ago</p></div>')
    $("").prepend('<li class="notification-item"></li><li><hr class="dropdown-divider"></li>');
        
    
    document.getElementById("monitor-alert-schedule").append("");
    li.textContent = `${message}`;
});

connection.on("ErrorIndexAlertMonitorJob", function (message) {
    var li = document.createElement("li");
    document.getElementById("errorlogIndex-alert").appendChild(li);
    li.textContent = `${message}`;
});