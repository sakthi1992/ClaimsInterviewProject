import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:7286/api/"
});

export default api;