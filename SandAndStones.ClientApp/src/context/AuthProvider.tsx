import { createContext, useState, PropsWithChildren, useMemo, useEffect, useContext } from "react";
import { axiosInstance } from "../hooks/useAxios";

interface AuthObject {
    token: string | null;
    setContextToken: (newToken: string) => void
}

const AuthContext = createContext<AuthObject | null>(null);

const AuthProvider = (props: PropsWithChildren) => {
    const [token, setToken] = useState(localStorage.getItem("token"));

    const setContextToken = (newToken : string) => {
        setToken(newToken);
    };

    useEffect(() => {
        if (token) {
            axiosInstance.defaults.headers.common["Authorization"] = "Bearer " + token;
            localStorage.setItem('token', token);
        } else {
            delete axiosInstance.defaults.headers.common["Authorization"];
            localStorage.removeItem('token')
        }
    }, [token]);
    
    const contextValue = useMemo(
        () => ({
            token,
            setContextToken,
        }),
        [token]
    );

    return (
        <AuthContext.Provider value={contextValue}>
            {props.children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => {
    return useContext(AuthContext);
};

export default AuthProvider;