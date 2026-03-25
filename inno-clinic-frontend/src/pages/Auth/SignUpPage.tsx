import { Button } from "@/components/ui/button";
import { Card, CardAction, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Field, FieldGroup, FieldLabel } from "@/components/ui/field";
import { Link } from "react-router-dom";

export default function SignUpPage() {
    return (
        <Card className="w-full max-w-md mx-auto">
            <CardHeader>
                <CardTitle>Create an account</CardTitle>
                <CardDescription>Enter your details below to create your account</CardDescription>
                <CardAction>
                    <Button variant="link" asChild>
                        <Link to="/signin">Sign In</Link>
                    </Button>
                </CardAction>
            </CardHeader>
            <form>
                <CardContent className="flex flex-col gap-6 pb-6">
                    <FieldGroup>
                        <Field>
                            <FieldLabel htmlFor="email">Email address</FieldLabel>
                            <Input
                                id="email"
                                type="email"
                                placeholder="name@example.com"
                                required
                            />
                        </Field>
                        <Field>
                            <FieldLabel htmlFor="password">Password</FieldLabel>
                            <Input id="password" type="password" required />
                        </Field>
                        <Field>
                            <FieldLabel htmlFor="confirm-password">Confirm Password</FieldLabel>
                            <Input id="confirm-password" type="password" required />
                        </Field>
                    </FieldGroup>

                    <div data-slot="alert" role="alert" className="group/alert relative w-full rounded-none border border-border/50 bg-muted/30 p-4 text-xs/relaxed text-muted-foreground ring-1 ring-border/10">
                        <div data-slot="alert-title" className="mb-0.5 font-medium leading-none tracking-tight text-foreground">
                            Privacy Policy
                        </div>
                        <div data-slot="alert-description" className="text-muted-foreground">
                            By creating an account, you agree to our Terms of Service and Privacy Policy.
                        </div>
                    </div>
                </CardContent>
                <CardFooter>
                    <Button type="submit" className="w-full">
                        Create account
                    </Button>
                </CardFooter>
            </form>
        </Card>
    );
}

