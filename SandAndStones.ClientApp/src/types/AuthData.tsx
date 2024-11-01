export interface AuthData {
    isAuthenticated: boolean
    logUserIn: () => void;
    logUserOut: () => void;
    checkTokenValidity: () => void;
}