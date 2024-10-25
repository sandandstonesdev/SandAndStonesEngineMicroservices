import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthProvider";
import { PropsWithChildren } from "react";

interface AuthObjectToken {
    token: string | null
}

type ProtectedRouteProps = PropsWithChildren;

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const { token } = useAuth() as AuthObjectToken;

    if (!token) {
        return <Navigate to="/login" />;
    }

    return children;
}