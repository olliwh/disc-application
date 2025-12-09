CREATE 
    (:Company {
        id: 1,
        name: 'TechCorp',
        location: 'Copenhagen',
        business_field: 'Software'
    }),

    (d1:Department {
        id: 1,
        name: 'HR',
        description: 'Manages all aspects of employee life cycle, including recruitment, benefits, training, and workplace culture.'
    }),
        (dp4:DiscProfile {
        id: 4,
        name: 'Conscientiousness',
        color: '#FFFF00',
        description: 'Analytical, detail-oriented'
    }),

    (po1:Position {
        id: 1,
        name: 'Software Engineer',
        description: 'Designs, develops, tests, and maintains software applications. 
        Collaborates with cross-functional teams to identify and prioritize project 
        requirements.'
    }),
        (ur4:UserRole {
        id: 4,
        name: 'ReadOnly',
        description: 'Can view data but not modify it'
    }),

    (ci1:CompletionInterval {
        id: 1,
        time_to_complete: '1-2 hours'
    }),
        (ur4:UserRole {
        id: 4,
        name: 'ReadOnly',
        description: 'Can view data but not modify it'
    }),

    (ci1:CompletionInterval {
        id: 1,
        time_to_complete: '1-2 hours'
    }),