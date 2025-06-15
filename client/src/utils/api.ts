import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL;
const baseAPIURL = baseURL + "api";

export const apiClient = axios.create({ baseURL: baseAPIURL });
export const apiClientProfiler = axios.create({ baseURL });
