import { useEffect, useState } from "react";
import api from "../../services/api";

function MenuItemSection({ menu }) {

    const [items, setItems] = useState([]);

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

    const [error, setError] = useState("");

    const [showForm, setShowForm] = useState(false);
    const [editingItem, setEditingItem] = useState(null);

    const [form, setForm] = useState({
        name: "",
        category: "",
        price: "",
        isAvailable: true
    });


    useEffect(() => {

        if (menu) {
            loadMenuItems();
        }

    }, [menu]);


    async function loadMenuItems() {

        setLoading(true);
        setError("");

        try {

            const response = await api.get(
                "/MenuItem/see-all-menuItems"
            );

            const allItems = response.data || [];

            const menuItems = allItems.filter(
                item => item.menuId === menu.id
            );

            setItems(menuItems);

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to load menu items"
            );

        } finally {

            setLoading(false);

        }
    }


    function resetForm() {

        setForm({
            name: "",
            category: "",
            price: "",
            isAvailable: true
        });

        setEditingItem(null);
        setShowForm(false);

    }


    function handleChange(e) {

        const { name, value } = e.target;

        setForm(prev => ({
            ...prev,
            [name]: value
        }));

    }


    function handleAvailabilityChange(e) {

        setForm(prev => ({
            ...prev,
            isAvailable: e.target.checked
        }));

    }


    function handleAddItem() {

        setEditingItem(null);

        setForm({
            name: "",
            category: "",
            price: "",
            isAvailable: true
        });

        setShowForm(true);

    }


    function handleEditItem(item) {

        setEditingItem(item);

        setForm({
            name: item.name,
            category: item.category,
            price: item.price,
            isAvailable: item.isAvailable
        });

        setShowForm(true);

    }


    async function handleSubmit(e) {

        e.preventDefault();

        setSaving(true);
        setError("");

        try {

            if (editingItem) {

                const response = await api.patch(
                    `/MenuItem/update-menuItem?id=${editingItem.id}`,
                    {
                        name: form.name,
                        category: form.category,
                        price: Number(form.price),
                        isAvailable: form.isAvailable
                    }
                );

                setItems(prev =>
                    prev.map(item =>
                        item.id === editingItem.id
                            ? response.data
                            : item
                    )
                );

            } else {

                const response = await api.post(
                    "/MenuItem/create-menuItem",
                    {
                        name: form.name,
                        category: form.category,
                        price: Number(form.price),
                        isAvailable: form.isAvailable,
                        menuId: menu.id
                    }
                );

                setItems(prev => [
                    ...prev,
                    response.data
                ]);

            }

            resetForm();

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to save menu item"
            );

        } finally {

            setSaving(false);

        }
    }


    async function handleDeleteItem(id) {

        const confirmed = window.confirm(
            "Are you sure you want to delete this menu item?"
        );

        if (!confirmed) {
            return;
        }

        setError("");

        try {

            await api.delete(
                `/MenuItem/delete-menuItem?id=${id}`
            );

            setItems(prev =>
                prev.filter(item => item.id !== id)
            );

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to delete menu item"
            );

        }

    }


    if (!menu) {
        return null;
    }


    return (

        <div className="mt-8">

            {/* HEADER */}

            <div className="flex justify-between items-center">

                <div>

                    <p className="text-sm text-indigo-600 font-semibold uppercase">
                        Food Items
                    </p>

                    <h3 className="text-2xl font-bold mt-1">
                        Menu Items
                    </h3>

                </div>


                {!showForm && (

                    <button
                        onClick={handleAddItem}
                        className="bg-indigo-600 hover:bg-indigo-700 text-white px-5 py-3 rounded-lg"
                    >
                        + Add Menu Item
                    </button>

                )}

            </div>


            {/* ERROR */}

            {error && (

                <div className="bg-red-100 text-red-700 p-4 rounded-lg mt-5">
                    {error}
                </div>

            )}


            {/* FORM */}

            {showForm && (

                <div className="border rounded-xl p-6 mt-6">

                    <h4 className="text-xl font-bold mb-6">

                        {editingItem
                            ? "Update Menu Item"
                            : "Add Menu Item"}

                    </h4>


                    <form onSubmit={handleSubmit}>

                        {/* NAME */}

                        <label className="block font-medium mb-2">
                            Name
                        </label>

                        <input
                            name="name"
                            value={form.name}
                            onChange={handleChange}
                            required
                            className="border p-3 rounded-lg w-full mb-5"
                            placeholder="e.g. Chicken Momo"
                        />


                        {/* CATEGORY */}

                        <label className="block font-medium mb-2">
                            Category
                        </label>

                        <input
                            name="category"
                            value={form.category}
                            onChange={handleChange}
                            required
                            className="border p-3 rounded-lg w-full mb-5"
                            placeholder="e.g. Momo"
                        />


                        {/* PRICE */}

                        <label className="block font-medium mb-2">
                            Price
                        </label>

                        <input
                            name="price"
                            type="number"
                            min="0"
                            step="0.01"
                            value={form.price}
                            onChange={handleChange}
                            required
                            className="border p-3 rounded-lg w-full mb-5"
                            placeholder="e.g. 250"
                        />


                        {/* AVAILABLE */}

                        <label className="flex items-center gap-2 mb-7">

                            <input
                                type="checkbox"
                                checked={form.isAvailable}
                                onChange={handleAvailabilityChange}
                            />

                            Item is available

                        </label>


                        {/* BUTTONS */}

                        <div className="flex gap-4">

                            <button
                                type="submit"
                                disabled={saving}
                                className="bg-indigo-600 hover:bg-indigo-700 text-white px-6 py-3 rounded-lg disabled:opacity-50"
                            >

                                {saving
                                    ? "Saving..."
                                    : editingItem
                                        ? "Update Item"
                                        : "Add Item"}

                            </button>


                            <button
                                type="button"
                                onClick={resetForm}
                                className="bg-gray-200 hover:bg-gray-300 px-6 py-3 rounded-lg"
                            >
                                Cancel
                            </button>

                        </div>

                    </form>

                </div>

            )}


            {/* LOADING */}

            {loading && (

                <p className="text-gray-500 mt-6">
                    Loading menu items...
                </p>

            )}


            {/* EMPTY */}

            {!loading && items.length === 0 && !showForm && (

                <div className="border-2 border-dashed border-gray-200 rounded-xl p-8 text-center mt-6">

                    <div className="text-4xl">
                        🍽️
                    </div>

                    <h4 className="text-xl font-semibold mt-3">
                        No menu items yet
                    </h4>

                    <p className="text-gray-500 mt-2">
                        Add your first food item to the menu.
                    </p>

                </div>

            )}


            {/* ITEMS */}

            {!loading && items.length > 0 && (

                <div className="grid grid-cols-1 md:grid-cols-2 gap-5 mt-6">

                    {items.map(item => (

                        <div
                            key={item.id}
                            className="border rounded-xl p-6"
                        >

                            <div className="flex justify-between items-start">

                                <div>

                                    <h4 className="text-xl font-bold">
                                        {item.name}
                                    </h4>

                                    <p className="text-gray-500 mt-1">
                                        {item.category}
                                    </p>

                                </div>


                                {item.isAvailable ? (

                                    <span className="bg-green-100 text-green-700 px-3 py-1 rounded-full text-sm">
                                        Available
                                    </span>

                                ) : (

                                    <span className="bg-red-100 text-red-700 px-3 py-1 rounded-full text-sm">
                                        Unavailable
                                    </span>

                                )}

                            </div>


                            <div className="mt-5">

                                <p className="text-gray-500 text-sm">
                                    Price
                                </p>

                                <p className="text-2xl font-bold">
                                    Rs. {item.price}
                                </p>

                            </div>


                            <div className="flex gap-3 mt-6">

                                <button
                                    onClick={() =>
                                        handleEditItem(item)
                                    }
                                    className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-lg"
                                >
                                    ✏️ Edit
                                </button>


                                <button
                                    onClick={() =>
                                        handleDeleteItem(item.id)
                                    }
                                    className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-lg"
                                >
                                    🗑️ Delete
                                </button>

                            </div>

                        </div>

                    ))}

                </div>

            )}

        </div>
    );
}

export default MenuItemSection;