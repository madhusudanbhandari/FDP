import { useState } from "react";
import api from "../../services/api";

function CreateRestaurantForm({ onCreated, onCancel }) {

    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");

    const [form, setForm] = useState({
        name: "",
        address: "",
        isOpen: true,
        rating: 0,
        capacity: 0,
        special: ""
    });


    function handleChange(e) {

        const { name, value } = e.target;

        setForm({
            ...form,
            [name]: value
        });

    }


    async function handleSubmit(e) {

        e.preventDefault();

        setSaving(true);
        setError("");

        try {

            const response = await api.post(
                "/Restaurant/create-restaurant",
                {
                    name: form.name,
                    address: form.address,
                    isOpen: form.isOpen,
                    rating: Number(form.rating),
                    capacity: Number(form.capacity),
                    special: form.special
                }
            );

            onCreated(response.data);

        } catch (error) {

            console.error(error);

            setError(
                error.response?.data?.message ||
                "Failed to create restaurant"
            );

        } finally {

            setSaving(false);

        }
    }


    return (

        <div className="bg-white rounded-2xl shadow-sm p-8 mt-8 max-w-3xl">

            <button
                onClick={onCancel}
                className="text-indigo-600 mb-6"
            >
                ← Back
            </button>


            <h2 className="text-3xl font-bold">
                Create Your Restaurant
            </h2>

            <p className="text-gray-500 mt-2 mb-8">
                Enter your restaurant information.
            </p>


            {error && (

                <div className="bg-red-100 text-red-700 p-4 rounded-lg mb-6">
                    {error}
                </div>

            )}


            <form onSubmit={handleSubmit}>

                <label className="block font-medium mb-2">
                    Restaurant Name
                </label>

                <input
                    name="name"
                    value={form.name}
                    onChange={handleChange}
                    required
                    className="border p-3 rounded-lg w-full mb-5"
                />


                <label className="block font-medium mb-2">
                    Address
                </label>

                <input
                    name="address"
                    value={form.address}
                    onChange={handleChange}
                    required
                    className="border p-3 rounded-lg w-full mb-5"
                />


                <label className="block font-medium mb-2">
                    Capacity
                </label>

                <input
                    name="capacity"
                    type="number"
                    min="0"
                    value={form.capacity}
                    onChange={handleChange}
                    required
                    className="border p-3 rounded-lg w-full mb-5"
                />


                <label className="block font-medium mb-2">
                    Speciality
                </label>

                <input
                    name="special"
                    value={form.special}
                    onChange={handleChange}
                    placeholder="e.g. Momo, Pizza, Burger"
                    required
                    className="border p-3 rounded-lg w-full mb-5"
                />


                <label className="flex items-center gap-2 mb-7">

                    <input
                        type="checkbox"
                        checked={form.isOpen}
                        onChange={(e) =>
                            setForm({
                                ...form,
                                isOpen: e.target.checked
                            })
                        }
                    />

                    Restaurant is open

                </label>


                <div className="flex gap-4">

                    <button
                        type="submit"
                        disabled={saving}
                        className="bg-indigo-600 hover:bg-indigo-700 text-white px-6 py-3 rounded-lg disabled:opacity-50"
                    >
                        {saving
                            ? "Creating..."
                            : "Create Restaurant"}
                    </button>


                    <button
                        type="button"
                        onClick={onCancel}
                        className="bg-gray-200 hover:bg-gray-300 px-6 py-3 rounded-lg"
                    >
                        Cancel
                    </button>

                </div>

            </form>

        </div>
    );
}

export default CreateRestaurantForm;