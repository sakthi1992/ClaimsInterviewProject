import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../services/api";

function ClaimDetails() {

    const { id } = useParams();
    const [claim, setClaim] = useState(null);

    useEffect(() => {
        api.get(`/claims/${id}`)
            .then(res => setClaim(res.data))
            .catch(err => console.log(err));
    }, [id]);

    if (!claim)
        return (
            <div className="container mt-5 text-center">
                <div className="spinner-border text-primary"></div>
            </div>
        );

    const getStatusBadge = (status) => {
        switch (status) {
            case "Approved":
                return "badge bg-success";
            case "Rejected":
                return "badge bg-danger";
            case "Submitted":
                return "badge bg-primary";
            default:
                return "badge bg-secondary";
        }
    };

    return (
        <div className="container mt-5">

            <div className="mb-3">
                <Link to="/" className="btn btn-outline-secondary">
                    ← Back to Claims
                </Link>
            </div>

            <div className="card shadow mx-auto" style={{ maxWidth: "800px" }}>
                <div className="card-body">

                    <h3 className="mb-4">Claim Details</h3>

                    <div className="row mb-3">
                        <div className="col-md-6">
                            <strong>Claim Number</strong>
                            <p>{claim.claimNumber}</p>
                        </div>

                        <div className="col-md-6">
                            <strong>Status</strong>
                            <p>
                                <span className={getStatusBadge(claim.status)}>
                                    {claim.status}
                                </span>
                            </p>
                        </div>
                    </div>

                    <div className="row mb-3">
                        <div className="col-md-6">
                            <strong>Member Name</strong>
                            <p>{claim.memberName}</p>
                        </div>

                        <div className="col-md-6">
                            <strong>Provider Name</strong>
                            <p>{claim.providerName}</p>
                        </div>
                    </div>

                    <div className="row">
                        <div className="col-md-6">
                            <strong>Amount</strong>
                            <p>${claim.amount}</p>
                        </div>

                        <div className="col-md-6">
                            <strong>Service Date</strong>
                            <p>{new Date(claim.serviceDate).toLocaleDateString()}</p>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    );
}

export default ClaimDetails;