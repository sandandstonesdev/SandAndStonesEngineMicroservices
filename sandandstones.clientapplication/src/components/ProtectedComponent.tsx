import { PropsWithChildren, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

import { useUserContext } from './UserContext';

type ProtectedRouteProps = PropsWithChildren;

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const user = useUserContext();
    const navigate = useNavigate();

    useEffect(() => {
        if (user === null) {
            navigate('/login', { replace: true });
        }
    }, [navigate, user]);

    return children;
}