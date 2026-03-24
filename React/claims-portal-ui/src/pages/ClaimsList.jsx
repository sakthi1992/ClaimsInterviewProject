import { useEffect, useState } from "react";
import api from "../services/api";
import { Link } from "react-router-dom";

function ClaimsList() {
    const [claims, setClaims] = useState([]);
    const [searchTerm, setSearchTerm] = useState("");
    const [statusFilter, setStatusFilter] = useState("All");
    const [sortConfig, setSortConfig] = useState({ key: "claimNumber", direction: "asc" });
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage, setItemsPerPage] = useState(5); // Adjust items per page as needed

    // Modal state
    const [showModal, setShowModal] = useState(false);
    const [selectedClaimId, setSelectedClaimId] = useState(null);
    const [note, setNote] = useState("");
    const [existingNoteId, setExistingNoteId] = useState(null);
    const [claimStatus, setClaimStatus] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const [totalItems, setTotalItems] = useState(0);
    const [totalPages, setTotalPages] = useState(0);

    const fetchClaims = () => {
        api.get("Claims/paged", {
            params: {
                page: currentPage,
                pageSize: itemsPerPage,
                searchTerm: searchTerm || "",
                status: statusFilter,
                sortBy: sortConfig.key,
                sortDirection: sortConfig.direction
            }
        })
            .then(res => {
                setClaims(res.data.items || []);
                setTotalItems(res.data.totalItems || 0);
                setTotalPages(res.data.totalPages || 0);
            })
            .catch(err => console.log(err));
    };

    // Main effect to fetch claims when parameters change
    // Using a small debounce for search to avoid excessive API calls
    useEffect(() => {
        const delayDebounceFn = setTimeout(() => {
            fetchClaims();
        }, 300);

        return () => clearTimeout(delayDebounceFn);
    }, [currentPage, searchTerm, statusFilter, sortConfig, itemsPerPage]);

    // Reset to first page when search, filter or itemsPerPage changes
    useEffect(() => {
        if (currentPage !== 1) {
            setCurrentPage(1);
        }
    }, [searchTerm, statusFilter, itemsPerPage]);

    const handleDelete = async (id) => {
        if (window.confirm("Are you sure you want to delete this claim?")) {
            try {
                await api.delete(`Claims/${id}`);
                fetchClaims();
            } catch (err) {
                console.error("Error deleting claim:", err);
                alert("Failed to delete claim.");
            }
        }
    };

    const handleOpenModal = async (id) => {
        setSelectedClaimId(id);
        setNote("");
        setExistingNoteId(null);

        // Set current status
        const claim = claims.find(c => c.id === id);
        if (claim) {
            setClaimStatus(claim.status || "Submitted");
        }

        setShowModal(true);

        try {
            const res = await api.get(`Claims/${id}/notes`);
            if (res.data && res.data.length > 0) {
                // Pre-fill with the first note found
                setNote(res.data[0].Note || res.data[0].note || "");
                setExistingNoteId(res.data[0].Id || res.data[0].id || null);
            }
        } catch (err) {
            console.error("Error fetching notes:", err);
        }
    };

    const handleSubmitNote = async (e) => {
        e.preventDefault();
        if (note.length < 1 || note.length > 500) {
            alert("Note must be between 1 and 500 characters.");
            return;
        }

        setIsSubmitting(true);
        try {
            // Update status first
            await api.put(`Claims/${selectedClaimId}/status`, `"${claimStatus}"`, {
                headers: { 'Content-Type': 'application/json' }
            });

            // Then update the note
            await api.put(`Claims/${selectedClaimId}/notes`, [
                {
                    Id: existingNoteId || "00000000-0000-0000-0000-000000000000",
                    ClaimId: selectedClaimId,
                    Note: note
                }
            ]);
            setShowModal(false);
            alert("Claim updated successfully!");
            fetchClaims(); // Refresh the list to show new status
        } catch (err) {
            console.error("Error updating claim:", err.response?.data?.errors || err.response?.data || err);
            alert("Failed to update claim.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleSort = (key) => {
        let direction = "asc";
        if (sortConfig.key === key && sortConfig.direction === "asc") {
            direction = "desc";
        }
        setSortConfig({ key, direction });
    };

    const getSortIcon = (key) => {
        if (sortConfig.key !== key) return "↕";
        return sortConfig.direction === "asc" ? "↑" : "↓";
    };

    return (
        <div className="container mt-5">

            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>Claims Portal</h2>

                <Link to="/create">
                    <button className="btn btn-primary">
                        Create Claim
                    </button>
                </Link>
            </div>

            <div className="card shadow mx-auto" style={{ maxWidth: "1200px" }}>
                <div className="card-body">

                    <div className="d-flex mb-4 gap-3">
                        <div className="flex-grow-1">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Search by Member, Provider or Claim #"
                                value={searchTerm}
                                onChange={(e) => setSearchTerm(e.target.value)}
                            />
                        </div>
                        <div style={{ minWidth: '200px' }}>
                            <select
                                className="form-select"
                                value={statusFilter}
                                onChange={(e) => setStatusFilter(e.target.value)}
                            >
                                <option value="All">All Statuses</option>
                                <option value="Draft">Draft</option>
                                <option value="Submitted">Submitted</option>
                                <option value="Under Review">Under Review</option>
                                <option value="Approved">Approved</option>
                                <option value="Rejected">Rejected</option>
                            </select>
                        </div>
                    </div>

                    <div className="table-responsive">
                        <table className="table table-striped table-hover">
                            <thead className="table-dark">
                                <tr>
                                    <th onClick={() => handleSort("claimNumber")} style={{ cursor: "pointer" }}>
                                        Claim Number {getSortIcon("claimNumber")}
                                    </th>
                                    <th onClick={() => handleSort("memberName")} style={{ cursor: "pointer" }}>
                                        Member {getSortIcon("memberName")}
                                    </th>
                                    <th onClick={() => handleSort("providerName")} style={{ cursor: "pointer" }}>
                                        Provider {getSortIcon("providerName")}
                                    </th>
                                    <th onClick={() => handleSort("amount")} style={{ cursor: "pointer" }}>
                                        Amount {getSortIcon("amount")}
                                    </th>
                                    <th onClick={() => handleSort("status")} style={{ cursor: "pointer" }}>
                                        Status {getSortIcon("status")}
                                    </th>
                                    <th>Actions</th>
                                </tr>
                            </thead>

                            <tbody>
                                {claims.length > 0 ? (
                                    claims.map(c => (
                                        <tr key={c.id}>
                                            <td>
                                                <Link to={`/claim/${c.id}`}>
                                                    {c.claimNumber}
                                                </Link>
                                            </td>
                                            <td>{c.memberName}</td>
                                            <td>{c.providerName}</td>
                                            <td>${c.amount}</td>
                                            <td>
                                                <span className={`badge ${c.status === "Approved" ? "bg-success" :
                                                    c.status === "Rejected" ? "bg-danger" : "bg-secondary"
                                                    }`}>
                                                    {c.status}
                                                </span>
                                            </td>
                                            <td>
                                                <div className="btn-group">
                                                    <button
                                                        className="btn btn-sm btn-outline-primary"
                                                        onClick={() => handleOpenModal(c.id)}
                                                        title="Add Note"
                                                    >
                                                        📝 Note
                                                    </button>
                                                    <button
                                                        className="btn btn-sm btn-outline-danger"
                                                        onClick={() => handleDelete(c.id)}
                                                        title="Delete Claim"
                                                    >
                                                        🗑️ Delete
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    ))
                                ) : (
                                    <tr>
                                        <td colSpan="6" className="text-center py-4">
                                            No claims found matching your criteria.
                                        </td>
                                    </tr>
                                )}
                            </tbody>

                        </table>
                    </div>

                    {/* Pagination UI */}
                    {totalPages > 1 && (
                        <div className="d-flex justify-content-between align-items-center mt-4 border-top pt-3">
                            <div className="d-flex align-items-center gap-3">
                                <select
                                    className="form-select form-select-sm w-auto"
                                    value={itemsPerPage}
                                    onChange={(e) => setItemsPerPage(Number(e.target.value))}
                                >
                                    <option value="5">5 per page</option>
                                    <option value="10">10 per page</option>
                                    <option value="20">20 per page</option>
                                    <option value="50">50 per page</option>
                                </select>
                                <div className="text-muted small">
                                    {totalItems > 0 && (
                                        <span>
                                            Showing {((currentPage - 1) * itemsPerPage) + 1} to {Math.min(currentPage * itemsPerPage, totalItems)} of {totalItems} claims
                                        </span>
                                    )}
                                </div>
                            </div>
                            <nav aria-label="Page navigation">
                                <ul className="pagination mb-0 justify-content-end">
                                    <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
                                        <button
                                            className="page-link"
                                            onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
                                            disabled={currentPage === 1}
                                        >
                                            Previous
                                        </button>
                                    </li>

                                    {[...Array(totalPages)].map((_, index) => {
                                        const pageNumber = index + 1;
                                        // Simple logic to show current page and neighbors if many pages
                                        if (
                                            totalPages <= 5 ||
                                            pageNumber === 1 ||
                                            pageNumber === totalPages ||
                                            (pageNumber >= currentPage - 1 && pageNumber <= currentPage + 1)
                                        ) {
                                            return (
                                                <li key={pageNumber} className={`page-item ${currentPage === pageNumber ? 'active' : ''}`}>
                                                    <button className="page-link" onClick={() => setCurrentPage(pageNumber)}>
                                                        {pageNumber}
                                                    </button>
                                                </li>
                                            );
                                        } else if (
                                            (pageNumber === currentPage - 2 && pageNumber > 1) ||
                                            (pageNumber === currentPage + 2 && pageNumber < totalPages)
                                        ) {
                                            return <li key={pageNumber} className="page-item disabled"><span className="page-link">...</span></li>;
                                        }
                                        return null;
                                    })}

                                    <li className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
                                        <button
                                            className="page-link"
                                            onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))}
                                            disabled={currentPage === totalPages}
                                        >
                                            Next
                                        </button>
                                    </li>
                                </ul>
                            </nav>
                        </div>
                    )}

                </div>
            </div>

            {/* Claim Note Modal */}
            {showModal && (
                <div className="modal fade show d-block" tabIndex="-1" style={{ backgroundColor: "rgba(0,0,0,0.5)" }}>
                    <div className="modal-dialog modal-dialog-centered">
                        <div className="modal-content border-0 shadow-lg">
                            <div className="modal-header bg-primary text-white">
                                <h5 className="modal-title">Add Claim Note</h5>
                                <button type="button" className="btn-close btn-close-white" onClick={() => setShowModal(false)}></button>
                            </div>
                            <form onSubmit={handleSubmitNote}>
                                <div className="modal-body">
                                    <div className="mb-3">
                                        <label className="form-label fw-bold">Note (1-500 characters)</label>
                                        <textarea
                                            className="form-control"
                                            rows="5"
                                            placeholder="Enter your note here..."
                                            value={note}
                                            onChange={(e) => setNote(e.target.value)}
                                            maxLength="500"
                                            required
                                        ></textarea>
                                        <div className="form-text text-end">
                                            {note.length} / 500
                                        </div>
                                    </div>

                                    <div className="mb-3">
                                        <label className="form-label fw-bold">Update Status</label>
                                        <select
                                            className="form-select"
                                            value={claimStatus}
                                            onChange={(e) => setClaimStatus(e.target.value)}
                                        >
                                            <option value="Draft">Draft</option>
                                            <option value="Drafted">Drafted</option>
                                            <option value="Submitted">Submitted</option>
                                            <option value="Under Review">Under Review</option>
                                            <option value="Approved">Approved</option>
                                            <option value="Rejected">Rejected</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="modal-footer bg-light">
                                    <button type="button" className="btn btn-secondary" onClick={() => setShowModal(false)}>Cancel</button>
                                    <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
                                        {isSubmitting ? (
                                            <>
                                                <span className="spinner-border spinner-border-sm me-2"></span>
                                                Submitting...
                                            </>
                                        ) : "Submit Note"}
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            )}

        </div>
    );
}

export default ClaimsList;