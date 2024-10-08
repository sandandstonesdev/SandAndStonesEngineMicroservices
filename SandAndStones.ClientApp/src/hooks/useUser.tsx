import { useContext } from "react";
import { useLocalStorage } from "./useLocalStorage";
import { AuthContext } from "./useAuth";
import { User } from "../types/User";

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