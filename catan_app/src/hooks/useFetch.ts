import { useState, useCallback } from 'react';
import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';
import apiClient from "../apiClient";

interface UseFetchResult<T> {
    data: T | null;
    error: string | null;
    loading: boolean;
    request: (url: string, method: 'get' | 'post' | 'put' | 'delete', data?: any) => Promise<T>;
}

const useFetch = <T = unknown>(initialUrl?: string, options?: AxiosRequestConfig): UseFetchResult<T> => {
    const [data, setData] = useState<T | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    const request = useCallback(async (url: string, method: 'get' | 'post' | 'put' | 'delete', requestData?: any) => {
        setLoading(true);
        if (initialUrl !== undefined)
            url = initialUrl.concat(url)

        try {
            const config = { ...options, method, url, data: requestData };
            const response: AxiosResponse<T> = await apiClient(config);
            setData(response.data);
            setError(null);
            return response.data;
        } catch (err) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data || 'An unexpected error occurred.');
            } else {
                setError('An unexpected error occurred.');
            }
            setData(null);
            throw err;
        } finally {
            setLoading(false);
        }
    }, [options]);

    return { data, error, loading, request };
};

export default useFetch;