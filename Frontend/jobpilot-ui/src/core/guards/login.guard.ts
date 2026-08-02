import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { Token } from '../services/token';

export const loginGuard: CanActivateFn = () => {

    const token = inject(Token);
    const router = inject(Router);

    if (token.isLoggedIn()) {

        router.navigate(['/dashboard']);

        return false;

    }

    return true;

};