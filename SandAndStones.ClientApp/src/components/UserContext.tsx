import { createContext, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { User } from '../types/User';

export const UserContext = createContext<User | null>(null);

export function useUserContext() {
    const user = useContext(UserContext);
    const navigate = useNavigate();

    if (user === null) {
        navigate('/login', { replace: true });
    }

    return user;
}