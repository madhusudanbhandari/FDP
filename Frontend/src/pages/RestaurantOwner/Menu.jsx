import { useEffect, useState } from "react";
import api from "../../services/api";
import MenuItemSection from "./MenuItem";

function MenuSection({ restaurant }) {

    const [menu, setMenu] = useState(null);

    const [menuLoading, setMenuLoading] = useState(false);
    const [menuError, setMenuError] = useState("");


    useEffect(() => {

        loadMenu();

    }, [restaurant]);


    async function loadMenu() {

        if (!restaurant) {
            return;
        }

        setMenuLoading(true);
        setMenuError("");

        try {

            const response = await api.get(
                "Menu/get-all-menus"
            );

            const menus = response.data || [];

            const restaurantMenu = menus.find(
                item =>
                    item.restaurantId === restaurant.id
            );

            setMenu(restaurantMenu || null);

        } catch (error) {

            console.error(error);

            setMenuError(
                error.response?.data?.message ||
                "Failed to load menu"
            );

        } finally {

            setMenuLoading(false);

        }
    }


    async function handleCreateMenu() {

        setMenuError("");
        setMenuLoading(true);

        try {

            const response = await api.post(
                `/Menu/create-menu?restaurantId=${restaurant.id}`
            );

            setMenu(response.data);

        } catch (error) {

            console.error(error);

            setMenuError(
                error.response?.data?.message ||
                "Failed to create menu"
            );

        } finally {

            setMenuLoading(false);

        }
    }


    async function handleDeleteMenu() {

        if (!menu) {
            return;
        }

        const confirmed = window.confirm(
            "Are you sure you want to delete your menu?"
        );

        if (!confirmed) {
            return;
        }

        setMenuError("");
        setMenuLoading(true);

        try {

            await api.delete(
                `/Menu/delete-menu?id=${menu.id}`
            );

            setMenu(null);

        } catch (error) {

            console.error(error);

            setMenuError(
                error.response?.data?.message ||
                "Failed to delete menu"
            );

        } finally {

            setMenuLoading(false);

        }
    }


    return (

        <div className="bg-white rounded-2xl shadow-sm p-8 mt-8">

            <div className="flex justify-between items-center">

                <div>

                    <p className="text-sm text-indigo-600 font-semibold uppercase">
                        Food Management
                    </p>

                    <h2 className="text-2xl font-bold mt-1">
                        Your Menu
                    </h2>

                </div>


                {!menu && !menuLoading && (

                    <button
                        onClick={handleCreateMenu}
                        className="bg-indigo-600 hover:bg-indigo-700 text-white px-5 py-3 rounded-lg"
                    >
                        + Create Menu
                    </button>

                )}

            </div>


            {menuError && (

                <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-5">
                    {menuError}
                </div>

            )}


            {menuLoading && (

                <p className="text-gray-500 mt-6">
                    Loading menu...
                </p>

            )}


            {!menu && !menuLoading && (

                <div className="border-2 border-dashed border-gray-200 rounded-xl p-8 text-center mt-6">

                    <div className="text-4xl">
                        🍔
                    </div>

                    <h3 className="text-xl font-semibold mt-3">
                        No menu yet
                    </h3>

                    <p className="text-gray-500 mt-2">
                        Create a menu to start adding food items.
                    </p>

                </div>

            )}


            {menu && (

                <div className="border rounded-xl p-6 mt-6">

                    <div className="flex justify-between items-center">

                        <div>

                            <h3 className="text-xl font-bold">
                                Main Menu
                            </h3>

                            <p className="text-gray-500 mt-1">
                                Menu ID: #{menu.id}
                            </p>

                            <p className="text-gray-500">
                                Restaurant ID: #{menu.restaurantId}
                            </p>

                        </div>


                        <button
                            onClick={handleDeleteMenu}
                            disabled={menuLoading}
                            className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-lg disabled:opacity-50"
                        >
                            {menuLoading
                                ? "Deleting..."
                                : "🗑️ Delete Menu"}
                        </button>

                    </div>

                <MenuItemSection menu={menu} />
                </div>

            )}

        </div>
    );
}

export default MenuSection;