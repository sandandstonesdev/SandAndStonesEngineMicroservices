import { createContext, useEffect, } from 'react';

import { User } from '../types/User';
import { useUser } from './useUser';

interface AuthContext {
    user: User | null;
    setUser: (user: User | null) => void;
}

export const AuthContext = createContext<AuthContext>({
    user: null,
    setUser: () => { },
});

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