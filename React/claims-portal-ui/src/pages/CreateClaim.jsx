import { useState } from "react";
import api from "../services/api";
import { useNavigate } from "react-router-dom";

function CreateClaim() {

    const navigate = useNavigate();

    const [form, setForm] = useState({
        memberName: "",
        providerName: "",
        amount: "",
        serviceDate: ""
    });

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        await api.post("/claims", form);
        navigate("/");
    };

    return (
        <div className="container mt-5">

            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Claims Portal</h2>

                <button
                    className="btn btn-secondary"
                    onClick={() => navigate(-1)}
                >
                    ← Back
                </button>
            </div>

            <div className="card shadow mx-auto" style={{ maxWidth: "500px" }}>

                <div className="card-body">

                    <h4 className="mb-4">Create Claim</h4>

                    <form onSubmit={handleSubmit}>

                        <div className="mb-3">
                            <label className="form-label">Member Name</label>
                            <input
                                type="text"
                                className="form-control"
                                name="memberName"
                                placeholder="Enter member name"
                                onChange={handleChange}
                                required
                            />
                        </div>

                        <div className="mb-3">
                            <label className="form-label">Provider Name</label>
                            <input
                                type="text"
                                className="form-control"
                                name="providerName"
                                placeholder="Enter provider name"
                                onChange={handleChange}
                                required
                            />
                        </div>

                        <div className="mb-3">
                            <label className="form-label">Amount</label>
                            <input
                                type="number"
                                className="form-control"
                                name="amount"
                                placeholder="Enter amount"
                                onChange={handleChange}
                                required
                            />
                        </div>

                        <div className="mb-3">
                            <label className="form-label">Service Date</label>
                            <input
                                type="date"
                                className="form-control"
                                name="serviceDate"
                                onChange={handleChange}
                                required
                            />
                        </div>

                        <button className="btn btn-primary w-100">
                            Submit Claim
                        </button>

                    </form>

                </div>

            </div>

        </div>
    );
}

export default CreateClaim;