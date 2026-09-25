import {
    HubConnectionBuilder,
    LogLevel
} from "@microsoft/signalr";

let connection = null;

export async function startNotificationHub(onNotification) {
    const token = localStorage.getItem("token");

    if (!token) {
        return;
    }

    connection = new HubConnectionBuilder()
        .withUrl("http://localhost:5107/notificationHub", {
            accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

    connection.on("ReceiveNotification", (notification) => {
        console.log("REAL-TIME NOTIFICATION:", notification);

        onNotification(notification);
    });

    try {
        await connection.start();

        console.log("Notification SignalR connected");
    } catch (error) {
        console.error(
            "Notification SignalR connection failed:",
            error
        );
    }
}

export async function stopNotificationHub() {
    if (connection) {
        await connection.stop();
        connection = null;
    }
}