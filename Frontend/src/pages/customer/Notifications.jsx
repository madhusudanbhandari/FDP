import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../services/api";
import {
    startNotificationHub,
    stopNotificationHub
} from "../../services/notificationHub";

function Notifications() {
    const [notifications, setNotifications] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetchNotifications();

        // startNotificationHub((notification)=>{
        //     setNotifications((currentNotifications)=>[
        //         notification,
        //         ...currentNotifications
        //     ]);
        // });

        // return()=>{
        //     stopNotificationHub();
        // };
    }, []);

    async function fetchNotifications() {
        try {
            setLoading(true);
            setError("");

            const response = await api.get(
                "/Notification/my-notifications"
            );

            console.log("NOTIFICATIONS:", response.data);

            setNotifications(response.data || []);

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to load notifications"
            );
        } finally {
            setLoading(false);
        }
    }

    async function markAsRead(notificationId) {
        try {
            await api.patch(
                `/Notification/${notificationId}/read`
            );

            setNotifications((currentNotifications) =>
                currentNotifications.map((notification) =>
                    notification.id === notificationId
                        ? {
                              ...notification,
                              isRead: true
                          }
                        : notification
                )
            );

        } catch (error) {
            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to mark notification as read"
            );
        }
    }

    function formatDate(date) {
        return new Date(date).toLocaleString();
    }

    if (loading) {
        return (
            <div className="min-h-screen bg-gray-100 p-8">
                <div className="max-w-4xl mx-auto">
                    Loading notifications...
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

            <div className="max-w-4xl mx-auto">

                <Link
                    to="/dashboards"
                    className="text-indigo-600"
                >
                    ← Dashboard
                </Link>

                <div className="flex justify-between items-center mt-6">

                    <div>
                        <h1 className="text-3xl font-bold">
                            Notifications
                        </h1>

                        <p className="text-gray-500 mt-2">
                            Stay updated about your orders.
                        </p>
                    </div>

                    <button
                        onClick={fetchNotifications}
                        className="bg-gray-200 hover:bg-gray-300 px-4 py-2 rounded-lg"
                    >
                        Refresh
                    </button>

                </div>

                {error && (
                    <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-6">
                        {error}
                    </div>
                )}

                {notifications.length === 0 ? (

                    <div className="bg-white rounded-xl shadow p-10 mt-8 text-center">

                        <div className="text-5xl mb-4">
                            🔔
                        </div>

                        <h2 className="text-2xl font-bold">
                            No Notifications
                        </h2>

                        <p className="text-gray-500 mt-2">
                            You don't have any notifications yet.
                        </p>

                    </div>

                ) : (

                    <div className="space-y-4 mt-8">

                        {notifications.map((notification) => (

                            <div
                                key={notification.id}
                                className={`rounded-xl shadow p-5 ${
                                    notification.isRead
                                        ? "bg-white"
                                        : "bg-indigo-50 border-l-4 border-indigo-500"
                                }`}
                            >

                                <div className="flex justify-between items-start gap-4">

                                    <div>

                                        <p
                                            className={`text-lg ${
                                                notification.isRead
                                                    ? "text-gray-700"
                                                    : "font-semibold text-gray-900"
                                            }`}
                                        >
                                            {notification.message}
                                        </p>

                                        <p className="text-sm text-gray-400 mt-2">
                                            {formatDate(
                                                notification.createdAt
                                            )}
                                        </p>

                                    </div>

                                    {!notification.isRead && (
                                        <button
                                            onClick={() =>
                                                markAsRead(
                                                    notification.id
                                                )
                                            }
                                            className="text-indigo-600 hover:text-indigo-800 font-medium whitespace-nowrap"
                                        >
                                            Mark as read
                                        </button>
                                    )}

                                </div>

                                {!notification.isRead && (
                                    <div className="mt-3">

                                        <span className="inline-block bg-indigo-100 text-indigo-700 text-xs font-semibold px-2 py-1 rounded-full">
                                            New
                                        </span>

                                    </div>
                                )}

                            </div>

                        ))}

                    </div>

                )}

            </div>

        </div>
    );
}

export default Notifications;