import { getApiUrl } from "./apiConfig";

const baseUrl = getApiUrl();

async function get(url: string) {
    const requestOptions = {
        method: 'GET',
        headers: await getHeaders(),
    }

    console.log('🔗 Fetching:', baseUrl + url, requestOptions);
    const response = await fetch(baseUrl + url, requestOptions);
    console.log('🔗 Response:', response);
    return handleResponse(response);
}

async function post(url: string, body: {}) {
    const requestOptions = {
        method: 'POST',
        headers: await getHeaders(),
        body: JSON.stringify(body)
    }
    const response = await fetch(baseUrl + url, requestOptions);
    return handleResponse(response);
}

async function put(url: string, body: {}) {
    const requestOptions = {
        method: 'PUT',
        headers: await getHeaders(),
        body: JSON.stringify(body)
    }
    const response = await fetch(baseUrl + url, requestOptions);
    return handleResponse(response);
}

// ✅ NOUVELLE MÉTHODE PATCH AJOUTÉE
async function patch(url: string, body: {}) {
    const requestOptions = {
        method: 'PATCH',
        headers: await getHeaders(),
        body: JSON.stringify(body)
    }
    const response = await fetch(baseUrl + url, requestOptions);
    return handleResponse(response);
}

async function del(url: string) {
    const requestOptions = {
        method: 'DELETE',
        headers: await getHeaders()
    }
    const response = await fetch(baseUrl + url, requestOptions);
    return handleResponse(response);
}

async function getHeaders() {
    const headers = {
        'Content-type': 'application/json'
    } as any
    return headers;
}

async function handleResponse(response: Response) {
    const text = await response.text();
    let data;
    try {
        data = JSON.parse(text);
    } catch (error) {
        data = text;
    }

    if (response.ok) {
        return data || response.statusText;
    } else {
        const errorMessage = typeof data === "string" ? data : data.message || response.statusText;
        return {
            error: {
                status: response.status,
                message: errorMessage,
            },
        };
    }
}

export const fetchWrapper = {
    get,
    post,
    put,
    patch,
    del
}