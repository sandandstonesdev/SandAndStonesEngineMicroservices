import { createContext, useContext, useEffect, useState } from 'react';

import { User } from '../types/User';

interface AuthContext {
    user: User | null;
    setUser: (user: User | null) => void;
}

export const AuthContext = createContext<AuthContext>({
    user: null,
    setUser: () => { },
});

export const useLocalStorage = () => {
    const [value, setValue] = useState<string | null>(null);

    const setItem = (key: string, value: string) => {
        localStorage.setItem(key, value);
        setValue(value);
    };

    const getItem = (key: string) => {
        const value = localStorage.getItem(key);
        setValue(value);
        return value;
    };

    const removeItem = (key: string) => {
        localStorage.removeItem(key);
        setValue(null);
    };

    return { value, setItem, getItem, removeItem };
};

export const useUser = () => {
    const { user, setUser } = useContext(AuthContext);
    const { getItem, setItem } = useLocalStorage();

    const addUser = (user: User) => {
        setUser(user);
        setItem("user", JSON.stringify(user));
    };

    const removeUser = () => {
        setUser(null);
        setItem("user", "");
    };

    const getUser = () => {
        const userItem = getItem("user")
        if (userItem === null || userItem === "")
            return null;
        const user: User = JSON.parse(userItem)
        return user;
    };

    return { user, addUser, removeUser, getUser, getItem, setUser };
};

export const useAuth = () => {
    const { user, addUser, removeUser, setUser, getUser } = useUser();
    
    useEffect(() => {
        const user = getUser();
        if (user) {
            addUser(user);
        }
    }, [addUser, getUser]);

    return { user, addUser, removeUser, setUser };
};