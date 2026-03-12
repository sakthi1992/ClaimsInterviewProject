import { BrowserRouter, Routes, Route } from "react-router-dom";
import ClaimsList from "./pages/ClaimsList";
import CreateClaim from "./pages/CreateClaim";
import ClaimDetails from "./pages/ClaimDetails";

function App() {
  return (
    <BrowserRouter>
      <div className="container text-center mt-4">
        <h1 className="mb-4">Claims Portal</h1>

        <Routes>
          <Route path="/" element={<ClaimsList />} />
          <Route path="/create" element={<CreateClaim />} />
          <Route path="/claim/:id" element={<ClaimDetails />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;