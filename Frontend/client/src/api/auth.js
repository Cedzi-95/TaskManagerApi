import { default as axios } from 'axios';

const API_BASE_URL = 'http://localhost:5041';
const isDevelopment = true;

// 1. Enhanced Axios instance configuration
const authApi = axios.create({
    baseURL: 'http://localhost:5041',
    headers: {
      'Content-Type': 'application/json',
      'X-Content-Type-Options': 'nosniff',
      'X-Frame-Options': 'DENY'
    }
  });

// 2. Improved Request Interceptor
authApi.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('authToken');
        
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        // Logging for development
        if (isDevelopment) {
            console.log('Request:', {
                url: config.url,
                method: config.method,
                headers: config.headers,
                data: config.data
            });
        }

        return config;
    },
    (error) => {
        if (isDevelopment) {
            console.error('Request Error:', error);
        }
        return Promise.reject(error);
    }
);

// 3. Comprehensive Response Interceptor
authApi.interceptors.response.use(
    (response) => {
        if (isDevelopment) {
            console.log('Response:', {
                status: response.status,
                data: response.data,
                headers: response.headers
            });
        }
        return response;
    },
    async (error) => {
        if (isDevelopment) {
            console.error('Response Error:', error);
        }

        const originalRequest = error.config;
        const isUnauthorized = error.response?.status === 401;
        const hasToken = !!localStorage.getItem('authToken');

        // 4. Token Refresh Logic
        if (isUnauthorized && hasToken && !originalRequest._retry) {
            originalRequest._retry = true;
            
            try {
                const refreshToken = localStorage.getItem('refreshToken');
                const response = await axios.post(`${API_BASE_URL}/auth/refresh`, { refreshToken });
                
                localStorage.setItem('authToken', response.data.accessToken);
                localStorage.setItem('refreshToken', response.data.refreshToken);
                
                originalRequest.headers.Authorization = `Bearer ${response.data.accessToken}`;
                return authApi(originalRequest);
            } catch (refreshError) {
                // 5. Cleanup and redirect on refresh failure
                localStorage.removeItem('authToken');
                localStorage.removeItem('refreshToken');
                window.location.href = '/login?sessionExpired=true';
                return Promise.reject(refreshError);
            }
        }

        // 6. Enhanced Error Handling
        if (error.response) {
            // Server responded with error status (4xx, 5xx)
            switch (error.response.status) {
                case 400:
                    console.error('Bad Request:', error.response.data);
                    break;
                case 403:
                    window.location.href = '/unauthorized';
                    break;
                case 404:
                    console.error('Not Found:', error.config.url);
                    break;
                case 500:
                    console.error('Server Error:', error.response.data);
                    break;
                default:
                    console.error('Unhandled Error:', error);
            }
        } else if (error.request) {
            // Request was made but no response received
            console.error('Network Error:', 'No response received');
        } else {
            // Something else happened
            console.error('Request Setup Error:', error.message);
        }

        return Promise.reject(error);
    }
);

// 7. Security Considerations
if (!isDevelopment) {
    authApi.defaults.headers.common['X-Content-Type-Options'] = 'nosniff';
    authApi.defaults.headers.common['X-Frame-Options'] = 'DENY';
}

// FIXED: Auth functions with correct ASP.NET Identity API endpoints and structure
export const loginUser = async (email, password) => {
    try {
        const requestData = {
            email: email,
            password: password
        };
        
        console.log('Sending login request with data:', requestData);
        
        // Use the correct Identity API endpoint and structure
        const response = await authApi.post('/login', requestData);
        
        // Store the tokens from the response
        if (response.data.accessToken) {
            localStorage.setItem('authToken', response.data.accessToken);
        }
        if (response.data.refreshToken) {
            localStorage.setItem('refreshToken', response.data.refreshToken);
        }
        
        return response.data;
    } catch (error) {
        console.error('Login error:', error);
        // Log the actual request data for debugging
        if (error.config?.data) {
            console.error('Request data that failed:', error.config.data);
        }
        throw error;
    }
};

export const registerUser = async (email, password) => {
    try {
        const requestData = {
            email: email,
            password: password
        };
        
        console.log('Sending registration request with data:', requestData);
        
        // Use the correct Identity API endpoint and structure
        const response = await authApi.post('/register', requestData);
        return response.data;
    } catch (error) {
        console.error('Registration error:', error);
        throw error;
    }
};

// Additional helper function to logout
export const logoutUser = async () => {
    try {
        // Clear local storage
        localStorage.removeItem('authToken');
        localStorage.removeItem('refreshToken');
        
        // Optional: call logout endpoint if your API has one
        // await authApi.post('/logout');
        
        return true;
    } catch (error) {
        console.error('Logout error:', error);
        // Still clear local storage even if API call fails
        localStorage.removeItem('authToken');
        localStorage.removeItem('refreshToken');
        return true;
    }
};

// Helper function to check if user is authenticated
export const isAuthenticated = () => {
    return !!localStorage.getItem('authToken');
};

export default authApi;