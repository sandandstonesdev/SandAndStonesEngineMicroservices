import { createContext, PropsWithChildren, useContext, useState } from "react";
import { AuthData } from "../types/AuthData";
import { axiosInstance } from "../hooks/useAxios";

const AuthContext = createContext<AuthData>(
{
    isAuthenticated: false,
    logUserIn: () => { },
    logUserOut: () => { },
    checkTokenValidity: async () => { }
});

const AuthProvider = (props: PropsWithChildren) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

    const logUserIn = () => {
        setIsAuthenticated(true);
    };

    const logUserOut = () => {
        setIsAuthenticated(false);
    };
    
    const checkTokenValidity = async () => {
        if(isAuthenticated) {
            const response = await axiosInstance.get(
                `${import.meta.env.VITE_APP_BASE_URL}/userAuthorization/currenttokenvalid`
            );

            if (response.data.isValid !== true)
                setIsAuthenticated(false);
            setIsAuthenticated(response.data.isValid)
        }
    }

    return (
        <AuthContext.Provider value={{ isAuthenticated, logUserIn, logUserOut, checkTokenValidity }}>
            {props.children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => {
    return useContext(AuthContext);
};

export default AuthProvider;