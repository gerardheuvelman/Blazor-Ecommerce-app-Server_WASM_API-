redirectToCheckout = function (sessionId) {
    const stripe = Stripe("pk_test_51NDPqqLK5PL09tPNQBJdkCQXI6QdDABC2hlFNlyJnBFTHiBdJ7jP8dm5l3SukOmJURDSjRdvT0H5yv7YyyV9qq0i00H935hDC3");
    stripe.redirectToCheckout({sessionId: sessionId})
}

