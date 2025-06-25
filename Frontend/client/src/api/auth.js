import { default as axios } from 'axios';

const API_BASE_URL = 'http://localhost:5041';
const isDevelopment = true;

// Enhanced Axios instance configuration
const authApi = axios.create({
    baseURL: 'http://localhost:5041/api', // Added /api prefix - adjust if your backend expects it
    headers: {
        // In your axios.create config:
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('authToken')}` // Changed from 'token' to 'authToken'
        }
    },
    timeout: 10000 // Add timeout to prevent hanging requests
});

// Request Interceptor
authApi.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('authToken');

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        if (isDevelopment) {
            console.log('🚀 API Request:', {
                url: `${config.baseURL}${config.url}`,
                method: config.method?.toUpperCase(),
                hasToken: !!token,
                tokenPreview: token ? `${token.substring(0, 20)}...` : 'none',
                timestamp: new Date().toISOString()
            });
        }

        return config;
    },
    (error) => {
        console.error('❌ Request Error:', error);
        return Promise.reject(error);
    }
);

// Response Interceptor
authApi.interceptors.response.use(
    (response) => {
        if (isDevelopment) {
            console.log('✅ Response Success:', {
                status: response.status,
                url: response.config.url,
                dataSize: JSON.stringify(response.data || {}).length,
                timestamp: new Date().toISOString()
            });
        }
        return response;
    },
    async (error) => {
        console.error('❌ Response Error:', {
            status: error.response?.status,
            statusText: error.response?.statusText,
            message: error.response?.data?.message || error.message,
            url: error.config?.url,
            fullUrl: error.config ? `${error.config.baseURL}${error.config.url}` : 'unknown',
            timestamp: new Date().toISOString()
        });

        const originalRequest = error.config;
        const isUnauthorized = error.response?.status === 401;
        const hasRefreshToken = !!localStorage.getItem('refreshToken');

        // Token refresh logic for Identity API
        if (isUnauthorized && hasRefreshToken && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                console.log('🔄 Attempting token refresh...');
                const refreshToken = localStorage.getItem('refreshToken');

                // Identity API refresh endpoint - use base URL without /api prefix
                const refreshResponse = await axios.post(`${API_BASE_URL}/refresh`, {
                    refreshToken: refreshToken
                });

                console.log('✅ Token refresh successful');

                // Identity API returns access_token and refresh_token
                if (refreshResponse.data.accessToken) {
                    localStorage.setItem('authToken', refreshResponse.data.accessToken);
                    originalRequest.headers.Authorization = `Bearer ${refreshResponse.data.accessToken}`;
                }
                if (refreshResponse.data.refreshToken) {
                    localStorage.setItem('refreshToken', refreshResponse.data.refreshToken);
                }

                return authApi(originalRequest);
            } catch (refreshError) {
                console.error('❌ Token refresh failed:', refreshError);

                // Clear tokens and redirect
                localStorage.removeItem('authToken');
                localStorage.removeItem('refreshToken');
                localStorage.removeItem('userInfo'); // Clear any cached user info

                // Show user-friendly message
                alert('Your session has expired. Please log in again.');
                window.location.href = '/login?sessionExpired=true';
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

// FIXED: Identity API login function
export const loginUser = async (email, password) => {
    try {
        const requestData = {
            email: email,
            password: password
        };

        console.log('🔐 Attempting login for:', email);

        // Identity API login endpoint - use base URL without /api prefix
        const response = await axios.post(`${API_BASE_URL}/login?useCookies=false&useSessionCookies=false`, requestData);

        console.log('✅ Login successful');

        // Identity API returns accessToken and refreshToken
        if (response.data.accessToken) {
            localStorage.setItem('authToken', response.data.accessToken);
            console.log('💾 Access token stored');
        }
        if (response.data.refreshToken) {
            localStorage.setItem('refreshToken', response.data.refreshToken);
            console.log('💾 Refresh token stored');
        }

        // Test the token after a brief delay to ensure it's properly set
        setTimeout(async () => {
            try {
                await testToken();
            } catch (testError) {
                console.warn('⚠️ Initial token test failed, but login succeeded');
            }
        }, 500);

        return response.data;
    } catch (error) {
        console.error('❌ Login error:', error);
        console.error('Response data:', error.response?.data);

        // Provide more specific error messages
        if (error.response?.status === 400) {
            throw new Error('Invalid email or password');
        } else if (error.response?.status === 429) {
            throw new Error('Too many login attempts. Please try again later.');
        } else if (error.code === 'ECONNREFUSED') {
            throw new Error('Cannot connect to server. Please check if the server is running.');
        }

        throw error;
    }
};

// FIXED: Identity API register function
export const registerUser = async (email, password) => {
    try {
        const requestData = {
            email: email,
            password: password
        };

        console.log('📝 Attempting registration for:', email);

        // Identity API register endpoint - use base URL without /api prefix
        const response = await axios.post(`${API_BASE_URL}/register`, requestData);

        console.log('✅ Registration successful');
        return response.data;
    } catch (error) {
        console.error('❌ Registration error:', error);
        console.error('Response data:', error.response?.data);

        // Provide more specific error messages
        if (error.response?.status === 400) {
            const errorData = error.response.data;
            if (errorData.errors) {
                // Extract validation errors
                const errorMessages = Object.values(errorData.errors).flat();
                throw new Error(errorMessages.join(', '));
            }
            throw new Error('Registration failed. Please check your input.');
        }

        throw error;
    }
};

// Logout function
export const logoutUser = async () => {
    try {
        console.log('👋 Logging out...');

        // Optional: Call logout endpoint if your API has one
        // await authApi.post('/logout');

        // Clear tokens
        localStorage.removeItem('authToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userInfo');

        console.log('✅ Logout successful');
        return true;
    } catch (error) {
        console.error('❌ Logout error:', error);
        // Even if logout fails, clear local storage
        localStorage.removeItem('authToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userInfo');
        return true;
    }
};

// Check if user is authenticated
export const isAuthenticated = () => {
    const token = localStorage.getItem('authToken');
    if (!token) {
        console.log('🔒 No auth token found');
        return false;
    }

    console.log('🔑 Auth token found');
    return true;
};

// Test function to check if token is working
export const testToken = async () => {
    try {
        // Use authApi (which includes /api prefix) for this endpoint
        const response = await authApi.get('/manage/info');
        console.log('✅ Token is valid');
        return true;
    } catch (error) {
        console.error('❌ Token validation failed:', error);
        if (error.response?.status === 401) {
            console.log('🔄 Clearing invalid tokens');
            localStorage.removeItem('authToken');
            localStorage.removeItem('refreshToken');
        }
        return false;
    }
};

// Helper to check current auth status
export const checkAuthStatus = () => {
    const token = localStorage.getItem('authToken');
    const refreshToken = localStorage.getItem('refreshToken');

    const status = {
        hasAccessToken: !!token,
        hasRefreshToken: !!refreshToken,
        isAuthenticated: !!token,
        tokenLength: token?.length || 0,
        refreshTokenLength: refreshToken?.length || 0
    };

    console.log('📊 Auth Status:', status);
    return status;
};

// Helper function to get user info
export const getUserInfo = async () => {
    try {
        const response = await authApi.get('/manage/info');
        return response.data;
    } catch (error) {
        console.error('❌ Failed to get user info:', error);
        throw error;
    }
};

export default authApi;