﻿using Domain.Category.ValueObject;
using Domain.Common.Models;
using Domain.Product.ValueObject;

namespace Domain.Product.Events;

public record ProductCreatedEvent(ProductId ProductId, CategoryId CategoryId) : IDomainEvent;
