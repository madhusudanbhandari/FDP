import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import api from "../../services/api";

function RestaurantDetails() {
    const { id } = useParams();

    const [restaurant, setRestaurant] = useState(null);
    const [menu, setMenu] = useState(null);
    const [menuItems, setMenuItems] = useState([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        fetchRestaurant();
    }, [id]);

    async function fetchRestaurant() {
        try {
            setLoading(true);

            // 1. Get restaurant
            const restaurantResponse = await api.get(
                `/Restaurant/view-restaurant?id=${id}`
            );

            setRestaurant(restaurantResponse.data);

            // 2. Get all menus
            const menuResponse = await api.get(
                "/Menu/get-all-menus"
            );

            const restaurantMenu = menuResponse.data.find(
                menu => menu.restaurantId === Number(id)
            );

            setMenu(restaurantMenu || null);

            // 3. Get all menu items
            const menuItemsResponse = await api.get(
                "/MenuItem/see-all-menuItems"
            );

            const restaurantItems = menuItemsResponse.data.filter(
                item => item.menuId === restaurantMenu?.id
            );

            setMenuItems(restaurantItems);

        } catch (error) {
            console.error(error);
            setError("Failed to load restaurant");
        } finally {
            setLoading(false);
        }
    }

    async function addToCart(menuItemId) {
        try {
            await api.post("/Cart/items", {
                menuItemId: menuItemId,
                quantity: 1
            });

            alert("Item added to cart!");
        } catch (error) {
            console.error(error);
            alert("Failed to add item to cart");
        }
    }

    if (loading) {
        return <h2 className="p-8">Loading...</h2>;
    }

    if (error) {
        return (
            <p className="p-8 text-red-500">
                {error}
            </p>
        );
    }

    if (!restaurant) {
        return (
            <p className="p-8">
                Restaurant not found.
            </p>
        );
    }

    return (
        <div className="min-h-screen bg-gray-100 p-8">

            <div className="max-w-5xl mx-auto">

                <Link
                    to="/restaurants"
                    className="text-indigo-600"
                >
                    ← Back to Restaurants
                </Link>


                <div className="bg-white rounded-xl shadow p-8 mt-6">

                    <h1 className="text-3xl font-bold">
                        {restaurant.name}
                    </h1>

                    <p className="mt-3 text-gray-600">
                        📍 {restaurant.address}
                    </p>

                    <p className="mt-3">
                        ⭐ Rating: {restaurant.rating}
                    </p>

                    <p className="mt-3">
                        🍽️ Special: {restaurant.special}
                    </p>

                    <p className="mt-3">
                        👥 Capacity: {restaurant.capacity}
                    </p>

                </div>

                {/* Menu */}

                <div className="bg-white rounded-xl shadow p-8 mt-6">

                    <h2 className="text-2xl font-bold">
                        Menu
                    </h2>

                    {!menu && (
                        <p className="text-gray-500 mt-4">
                            This restaurant does not have a menu yet.
                        </p>
                    )}

                    {menu && menuItems.length === 0 && (
                        <p className="text-gray-500 mt-4">
                            No menu items available.
                        </p>
                    )}

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">

                        {menuItems.map(item => (
                            <div
                                key={item.id}
                                className="border rounded-lg p-5"
                            >

                                <h3 className="text-xl font-semibold">
                                    {item.name}
                                </h3>

                                <p className="text-gray-500">
                                    {item.category}
                                </p>

                                <p className="text-lg font-bold mt-2">
                                    Rs. {item.price}
                                </p>

                                {item.isAvailable ? (
                                    <button
                                        onClick={() => addToCart(item.id)}
                                        className="mt-4 bg-indigo-600 text-white px-4 py-2 rounded-lg"
                                    >
                                        Add to Cart
                                    </button>
                                ) : (
                                    <p className="text-red-500 mt-4">
                                        Currently unavailable
                                    </p>
                                )}

                            </div>
                        ))}

                    </div>

                </div>

            </div>

        </div>
    );
}

export default RestaurantDetails;