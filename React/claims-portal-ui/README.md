# Claims Portal - Frontend

This is the React-based frontend for the **Claims Management Portal**, built with **Vite, React, and Bootstrap**.

## Features

### Claims List
- **Server-side Pagination**: Efficiently handles large datasets by fetching only the required page.
- **Dynamic Search**: Real-time searching by Member Name, Provider Name, or Claim Number.
- **Status Filtering**: Filter claims by their current status (Draft, Submitted, Approved, etc.).
- **Multi-column Sorting**: Clickable table headers to sort by Claim Number, Member, Provider, Amount, or Status.
- **Configurable Page Size**: Choose between 5, 10, 20, or 50 items per page.

### Claim Management
- **Create Claim**: Simplified form to add new insurance claims.
- **Claim Details**: View complete claim information and associated notes.
- **Status Updates**: Update the status of a claim along with a note.

## Getting Started

### Installation
```bash
npm install
```

### Running in Development
```bash
npm run dev
```

### Build for Production
```bash
npm run build
```

## Project Structure
- `src/pages`: Main screen components (`ClaimsList`, `ClaimDetails`, `CreateClaim`).
- `src/services/api.js`: Axios configuration for backend communication.
- `src/App.jsx`: Main routing and layout.
