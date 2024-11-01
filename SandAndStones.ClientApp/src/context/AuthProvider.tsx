import { createContext, PropsWithChildren, useContext, useState } from "react";
import { AuthData } from "../types/AuthData";

const AuthContext = createContext<AuthData>(
{
    isAuthenticated: false,
    logUserIn: () => { },
    logUserOut: () => { }
});

const AuthProvider = (props: PropsWithChildren) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    
    const logUserIn = () => {
        setIsAuthenticated(true);
    };

    const logUserOut = () => {
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, logUserIn, logUserOut }}>
            {props.children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => {
    return useContext(AuthContext);
};

export default AuthProvider;